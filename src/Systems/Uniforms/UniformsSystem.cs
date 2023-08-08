using DefaultEcs.System;
using DefaultEcs;

namespace ShaderViewer.Systems.Uniforms;

internal abstract class UniformsSystem : ISystem<float>
{
	public UniformsSystem(World world)
	{
		world.SubscribeWorldComponentAddedOrChanged((World world, in Components.Uniforms uniforms) =>
		{
			IsEnabled = ShouldEnable(uniforms);
		});
		World = world;
	}

	public bool IsEnabled { get; set; }

	public World World { get; }

	public void Dispose()
	{
	}

	public void Update(float deltaTime)
	{
		if (!IsEnabled) return;
		Update(deltaTime, World.Get<Components.Uniforms>());
	}

	protected abstract void Update(float deltaTime, Components.Uniforms uniforms);

	protected abstract bool ShouldEnable(Components.Uniforms uniforms);
}