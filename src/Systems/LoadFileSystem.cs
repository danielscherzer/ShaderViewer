using DefaultEcs;
using OpenTK.Graphics.OpenGL4;
using ShaderViewer.Component;
using ShaderViewer.Helper;
using System;
using System.IO;
using Zenseless.OpenTK;
using Zenseless.Resources;

namespace ShaderViewer.Systems
{
	internal class LoadFileSystem
	{
		public LoadFileSystem(IResourceDirectory resourceDirectory, World world)
		{
			entity = world.CreateEntity();

			defaultVertexShader = (ShaderType.VertexShader, resourceDirectory.Resource("screenQuad.vert").AsString());
			defaultFragmentSourceCode = resourceDirectory.Resource("checker.frag").AsString();
			LoadShader(defaultFragmentSourceCode);
		}

		public event Action<string>? Loaded;

		public void LoadShaderFile(string fileName)
		{
			LoadFile(fileName);
			fileChangeSubscription?.Dispose();
			fileChangeSubscription = TrackedFileObservable.DelayedLoad(fileName)
				.Subscribe(fileName => LoadFile(fileName));
		}

		private readonly Entity entity;
		private readonly (ShaderType VertexShader, string) defaultVertexShader;
		private readonly string defaultFragmentSourceCode;
		private IDisposable? fileChangeSubscription = null;

		private void LoadFile(string fileName)
		{
			var sourceCode = File.ReadAllText(fileName);
			string dir = Path.GetDirectoryName(fileName) ?? "";
			sourceCode = GLSLhelper.Transformation.ExpandIncludes(sourceCode, include => File.ReadAllText(Path.Combine(dir, include)));
			entity.Set(new SourceCode(sourceCode));
			LoadShader(sourceCode);
			Loaded?.Invoke(fileName);
		}

		private void LoadShader(string shaderSource)
		{
			ShaderProgram Load(string shaderSource)
			{
				var fragment = (ShaderType.FragmentShader, shaderSource);
				return new ShaderProgram().CompileLink(defaultVertexShader, fragment);
			}

			if (entity.Has<ShaderProgram>()) entity.Get<ShaderProgram>().Dispose();
			try
			{
				entity.Set(Load(shaderSource));
				entity.Remove<Log>();
			}
			catch (ShaderException se)
			{
				entity.Set(new Log(se.Message));
				entity.Set(Load(defaultFragmentSourceCode));
			}
		}
	}
}