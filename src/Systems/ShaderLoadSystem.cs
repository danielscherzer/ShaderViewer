using DefaultEcs;
using DefaultEcs.System;
using ShaderViewer.Components.Shader;
using Zenseless.OpenTK;

namespace ShaderViewer.Systems;

internal sealed partial class ShaderLoadSystem : AEntitySetSystem<float>
{
	public ShaderLoadSystem(/*IGLFWGraphicsContext context, */World world) : base(world, CreateEntityContainer, true)
	{
		//this.context = context;

		world.SubscribeEntityComponentAdded((in Entity entity, in SourceCode sourceCode) => entity.Set(new CompileShader()));
		world.SubscribeEntityComponentChanged((in Entity entity, in SourceCode _, in SourceCode sourceCode) => entity.Set(new CompileShader()));
		//world.SubscribeComponentAdded((in Entity entity, in SourceCode sourceCode) => Load(entity, sourceCode));
		//world.SubscribeComponentChanged((in Entity entity, in SourceCode _, in SourceCode sourceCode) => Load(entity, sourceCode));
	}

	[Update]
	private static void Update(in Entity entity, in SourceCode sourceCode, in CompileShader _)
	{
		entity.Remove<CompileShader>();
		Load(entity, sourceCode);
	}

	private static void Load(in Entity entity, in SourceCode sourceCode)
	{
		try
		{
			//TODO: if (!context.IsCurrent) context.MakeCurrent();
			if (entity.Has<ShaderProgram>()) entity.Get<ShaderProgram>().Dispose(); //TODO: Dispose should happen automatically with resource
			entity.Set(ShaderResources.CompileLink(sourceCode));
			entity.Remove<Log>();
		}
		catch (ShaderException e)
		{
			entity.Set(new Log(e.Message));
		}
	}
}
