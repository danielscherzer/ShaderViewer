using DefaultEcs;
using ShaderViewer.Component;
using System;
using System.IO;
using System.Reactive.Linq;

namespace ShaderViewer.Systems;

internal static class ReadShaderSourceSystem
{
	public static void SubscribeReadShaderSourceSystem(this World world)
	{
		static void Load(Entity entity, string fileName)
		{
			void LoadFile(string fileName)
			{
				try
				{
					var sourceCode = File.ReadAllText(fileName);
					string dir = Path.GetDirectoryName(fileName) ?? "";
					sourceCode = GLSLhelper.Transformation.ExpandIncludes(sourceCode, includeName => File.ReadAllText(Path.Combine(dir, includeName)));
					entity.Set(new SourceCode(sourceCode));
				}
				catch (Exception e)
				{
					entity.Set(new Log($"Error loading include file with message: {e.Message}"));
				}
			}
			if (entity.Has<IDisposable>()) entity.Get<IDisposable>().Dispose();
			if (!File.Exists(fileName)) return;
			var fileChangeSubscription = DelayedLoad(fileName).Subscribe(fileName => LoadFile(fileName));
			entity.Set(fileChangeSubscription);
		}

		world.SubscribeComponentAdded((in Entity entity, in ShaderFile shaderFile) => Load(entity, shaderFile.Name));
		world.SubscribeComponentChanged((in Entity entity, in ShaderFile _, in ShaderFile shaderFile) => Load(entity, shaderFile.Name));
	}

	private static IObservable<string> DelayedLoad(string fileName)
	{
		//var scheduler = Scheduler.CurrentThread;
		return CreateFileChangeSequence(fileName)
			.Throttle(TimeSpan.FromSeconds(0.1f))
			.Delay(TimeSpan.FromSeconds(0.1f))
			//.SubscribeOn(scheduler)
			;
	}

	private static IObservable<string> CreateFileChangeSequence(string fileName)
	{
		var fullPath = Path.GetFullPath(fileName);
		return Observable.Return(fileName).Concat(
			Observable.Using(
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
			})
			);
	}
}