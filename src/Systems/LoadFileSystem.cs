using DefaultEcs;
using ShaderViewer.Component;
using ShaderViewer.Helper;
using System;
using System.IO;

namespace ShaderViewer.Systems;

internal class LoadFileSystem
{
	public LoadFileSystem(World world)
	{
		entity = world.CreateEntity();
	}

	public event Action<string>? Loaded;

	public void LoadShaderFile(string fileName)
	{
		fileChangeSubscription?.Dispose();
		fileChangeSubscription = TrackedFileObservable.DelayedLoad(fileName)
			.Subscribe(fileName => LoadFile(fileName));
	}

	private readonly Entity entity;
	private IDisposable? fileChangeSubscription = null;

	private void LoadFile(string fileName)
	{
		var sourceCode = File.ReadAllText(fileName);
		string dir = Path.GetDirectoryName(fileName) ?? "";
		sourceCode = GLSLhelper.Transformation.ExpandIncludes(sourceCode, include => File.ReadAllText(Path.Combine(dir, include)));
		entity.Set(new SourceCode(sourceCode));
		Loaded?.Invoke(fileName);
	}
}