using DefaultEcs;
using DefaultEcs.Resource;
using DefaultEcs.System;
using ShaderViewer.Component;
using Zenseless.OpenTK;

namespace ShaderViewer.System;

internal sealed partial class ShaderLoadSystem : AComponentSystem<float, SourceCode>
{
	public ShaderLoadSystem(World world) : base(world)
	{
		world.SubscribeWorldComponentAddedOrChanged((World _, in SourceCode sourceCode) => IsEnabled = true);
	}

	protected override void Update(float _, ref SourceCode sourceCode)
	{
		Load(sourceCode);
		IsEnabled = false;
	}

	private void Load(in SourceCode sourceCode)
	{
		try
		{
			//TODO: new ManagedResource < string, ShaderProgram>();

			if (World.Has<ShaderProgram>()) World.Get<ShaderProgram>().Dispose(); //TODO: Dispose should happen automatically with resource
			World.Set(Resources.CompileLink(sourceCode));
			World.Remove<Log>();
		}
		catch (ShaderException e)
		{
			World.Set(new Log(e.Message));
		}
	}
}
