using DefaultEcs.System;
using DefaultEcs;

namespace ShaderViewer.Systems.Uniforms;

internal abstract class UniformsSystem : ISystem<float>
{
	public UniformsSystem(World world)
	{
		world.SubscribeEntityComponentAddedOrChanged((in Entity _, in Components.Uniforms uniforms) =>
		{
			IsEnabled = ShouldEnable(uniforms);
			this.uniforms = uniforms;
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
		Update(deltaTime, uniforms);
	}

	protected abstract void Update(float deltaTime, Components.Uniforms uniforms);

	protected abstract bool ShouldEnable(Components.Uniforms uniforms);
	
	private Components.Uniforms uniforms;
}