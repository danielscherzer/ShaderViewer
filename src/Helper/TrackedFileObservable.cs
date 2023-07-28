using System;
using System.IO;
using System.Reactive.Linq;

namespace ShaderViewer.Helper;

internal static class TrackedFileObservable
{
	internal static IObservable<string> DelayedLoad(string fileName)
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
