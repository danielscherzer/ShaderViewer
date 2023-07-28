using DefaultEcs;
using DefaultEcs.System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using ShaderViewer.Component;
using Zenseless.OpenTK;
using Zenseless.Resources;

namespace ShaderViewer.Systems;

internal sealed partial class ShaderLoadSystem : AEntitySetSystem<float>
{
	public ShaderLoadSystem(IGLFWGraphicsContext context, World world) : base(world, CreateEntityContainer, true)
	{
		//this.context = context;

		world.SubscribeComponentAdded((in Entity entity, in SourceCode sourceCode) => reload = true);
		world.SubscribeComponentChanged((in Entity entity, in SourceCode _, in SourceCode sourceCode) => reload = true);
		//world.SubscribeComponentAdded((in Entity entity, in SourceCode sourceCode) => Load(entity, sourceCode));
		//world.SubscribeComponentChanged((in Entity entity, in SourceCode _, in SourceCode sourceCode) => Load(entity, sourceCode));

		var resourceDirectory = new ShortestMatchResourceDirectory(new EmbeddedResourceDirectory());
		defaultVertexShader = (ShaderType.VertexShader, resourceDirectory.Resource("screenQuad.vert").AsString());
		defaultFragmentSourceCode = resourceDirectory.Resource("checker.frag").AsString();
	}

	private readonly (ShaderType VertexShader, string) defaultVertexShader;
	private readonly string defaultFragmentSourceCode;
	//private readonly IGLFWGraphicsContext context;
	private bool reload = true; //TODO: for multi shader stuff move to entity and lock or do reload in subscribe (check thread!)

	[Update]
	private void Update(in Entity entity, in SourceCode sourceCode)
	{
		if (reload)
		{
			Load(entity, sourceCode);
			reload = false;
		}
	}

	private void Load(in Entity entity, in SourceCode sourceCode)
	{
		ShaderProgram CompileLink(string shaderSource)
		{
			var fragment = (ShaderType.FragmentShader, shaderSource);
			return new ShaderProgram().CompileLink(defaultVertexShader, fragment);
		}

		try
		{
			//if (!context.IsCurrent) context.MakeCurrent();
			if (entity.Has<ShaderProgram>()) entity.Get<ShaderProgram>().Dispose();
			entity.Set(CompileLink(sourceCode));
			entity.Remove<Log>();
		}
		catch (ShaderException se)
		{
			entity.Set(new Log(se.Message));
			entity.Set(CompileLink(defaultFragmentSourceCode));
		}
	}
}
