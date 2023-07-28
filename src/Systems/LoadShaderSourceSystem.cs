using DefaultEcs;
using ShaderViewer.Component;
using ShaderViewer.Helper;
using System;
using System.IO;

namespace ShaderViewer.Systems;

internal static class LoadShaderSourceSystem
{
	public static void Subscribe(World world)
	{
		world.SubscribeComponentAdded((in Entity entity, in ShaderFile shaderFile) => Load(entity, shaderFile.Name));
		world.SubscribeComponentChanged((in Entity entity, in ShaderFile _, in ShaderFile shaderFile) => Load(entity, shaderFile.Name));
	}

	private static void Load(Entity entity, string fileName)
	{
		void LoadFile(string fileName)
		{
			var sourceCode = File.ReadAllText(fileName);
			string dir = Path.GetDirectoryName(fileName) ?? "";
			sourceCode = GLSLhelper.Transformation.ExpandIncludes(sourceCode, include => File.ReadAllText(Path.Combine(dir, include)));
			entity.Set(new SourceCode(sourceCode));
		}
		if (entity.Has<IDisposable>()) entity.Get<IDisposable>().Dispose();
		var fileChangeSubscription = TrackedFileObservable.DelayedLoad(fileName)
			.Subscribe(fileName => LoadFile(fileName));
		entity.Set(fileChangeSubscription);
	}
}