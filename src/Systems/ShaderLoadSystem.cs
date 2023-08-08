using DefaultEcs;
using DefaultEcs.System;
using ShaderViewer.Components;
using Zenseless.OpenTK;

namespace ShaderViewer.Systems;

internal sealed partial class ShaderLoadSystem : AEntitySetSystem<float>
{
	public ShaderLoadSystem(World world) : base(world, CreateEntityContainer, true)
	{
		world.SubscribeEntityComponentAddedOrChanged((in Entity entity, in SourceCode sourceCode) => IsEnabled = true);
	}

	[Update]
	private void Update(in Entity entity, in SourceCode sourceCode)
	{
		Load(entity, sourceCode);
		IsEnabled = false;
	}

	private static void Load(in Entity entity, in SourceCode sourceCode)
	{
		try
		{
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
