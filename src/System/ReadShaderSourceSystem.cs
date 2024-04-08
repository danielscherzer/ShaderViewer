using DefaultEcs;
using ShaderViewer.Component;
using System;
using System.IO;
using System.Reactive.Linq;

namespace ShaderViewer.System;

internal static class ReadShaderSourceSystem
{
	internal static bool Load(World world, string fileName)
	{
		void Reload(string fileName)
		{
			try
			{
				var sourceCode = File.ReadAllText(fileName);
				string dir = Path.GetDirectoryName(fileName) ?? "";
				var observable = Observable.Empty<string>();
				string GetIncludeCode(string includeName)
				{
					var includeFile = Path.GetFullPath(Path.Combine(dir, includeName));
					observable = observable.Merge(CreateFileSystemWatcherObservable(includeFile).Select(_ => fileName)); // observe include changes, but return main shader file name
																														 //TODO: Test compile include and push errors to log
					return File.ReadAllText(includeFile);
				}

				sourceCode = GLSLhelper.Transformation.ExpandIncludes(sourceCode, GetIncludeCode);
				world.Set(new SourceCode(sourceCode));
				if (world.Has<IDisposable>()) world.Get<IDisposable>().Dispose();
				var fileChangeSubscription = observable.Merge(CreateFileSystemWatcherObservable(fileName))
					.Throttle(TimeSpan.FromSeconds(0.1f))
					.Delay(TimeSpan.FromSeconds(0.1f))
					//.ObserveOn(Scheduler.CurrentThread)
					.Subscribe(fileName => Reload(fileName));
				world.Set(fileChangeSubscription);
			}
			catch (Exception e)
			{
				world.Set(new Log(e.Message)); //TODO: put into log format
			}
		}
		if (!File.Exists(fileName)) return false;
		Reload(fileName);
		return true;
	}

	private static IObservable<string> CreateFileSystemWatcherObservable(string fileName)
	{
		var fullPath = Path.GetFullPath(fileName);
		return Observable.Using(
			() => new FileSystemWatcher(Path.GetDirectoryName(fullPath) ?? fullPath, Path.GetFileName(fullPath))
			{
				NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.CreationTime | NotifyFilters.FileName,
				EnableRaisingEvents = true,
			},
			watcher =>
			{
				var fileChanged = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(h => watcher.Changed += h, h => watcher.Changed -= h).Select(x => x.EventArgs.FullPath);
				var fileCreated = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(h => watcher.Created += h, h => watcher.Created -= h).Select(x => x.EventArgs.FullPath);
				var fileRenamed = Observable.FromEventPattern<RenamedEventHandler, RenamedEventArgs>(h => watcher.Renamed += h, h => watcher.Renamed -= h).Select(x => x.EventArgs.FullPath);
				return fileChanged.Merge(fileCreated).Merge(fileRenamed);
			});
	}
}