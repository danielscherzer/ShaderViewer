using DefaultEcs;
using DefaultEcs.System;
using OpenTK.Graphics.OpenGL4;
using ShaderViewer.Component;
using Zenseless.OpenTK;
using Zenseless.Resources;

namespace ShaderViewer.Systems
{
	internal sealed partial class ShaderLoadSystem : AEntitySetSystem<float>
	{
		public ShaderLoadSystem(IResourceDirectory resourceDirectory, World world) : base(world, CreateEntityContainer, true)
		{
			//TODO: try out window.Context.MakeCurrent();
			world.SubscribeComponentAdded((in Entity _, in SourceCode sourceCode) => reload = true);
			world.SubscribeComponentChanged((in Entity _, in SourceCode _, in SourceCode sourceCode) => reload = true);

			defaultVertexShader = (ShaderType.VertexShader, resourceDirectory.Resource("screenQuad.vert").AsString());
			defaultFragmentSourceCode = resourceDirectory.Resource("checker.frag").AsString();
		}

		private readonly (ShaderType VertexShader, string) defaultVertexShader;
		private readonly string defaultFragmentSourceCode;
		private bool reload = true; //TODO: for multi shader stuff move to entity and lock

		[Update]
		private void Update(in Entity entity, in SourceCode sourceCode)
		{
			if (reload)
			{
				if (entity.Has<ShaderProgram>()) entity.Get<ShaderProgram>().Dispose();
				Load(entity, sourceCode);
				reload = false;
			}
		}

		private void Load(in Entity entity, in SourceCode sourceCode)
		{
			try
			{
				entity.Set(LoadShader(sourceCode));
				entity.Remove<Log>();
			}
			catch (ShaderException se)
			{
				entity.Set(new Log(se.Message));
				entity.Set(LoadShader(defaultFragmentSourceCode));
			}
		}

		private ShaderProgram LoadShader(string shaderSource)
		{
			var fragment = (ShaderType.FragmentShader, shaderSource);
			return new ShaderProgram().CompileLink(defaultVertexShader, fragment);
		}
	}
}
