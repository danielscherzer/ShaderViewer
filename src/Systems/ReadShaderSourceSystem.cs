using DefaultEcs;
using ShaderViewer.Components;
using System;
using System.IO;
using System.Reactive.Linq;

namespace ShaderViewer.Systems;

internal static class ReadShaderSourceSystem
{
	public static void SubscribeReadShaderSourceSystem(this World world)
	{
		void Load(string fileName)
		{
			void LoadFile(string fileName)
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
						.Subscribe(fileName => LoadFile(fileName));
					world.Set(fileChangeSubscription);
				}
				catch (Exception e)
				{
					world.Set(new Log(e.Message));
				}
			}
			if (!File.Exists(fileName)) return;
			LoadFile(fileName);
		}

		world.SubscribeWorldComponentAddedOrChanged((World _, in ShaderFile shaderFile) => Load(shaderFile.Name));
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