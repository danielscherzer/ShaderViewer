using DefaultEcs;
using ShaderViewer.Component;
using ShaderViewer.Components.Shader;

namespace ShaderViewer.Systems.UniformUpdaters;

internal class TimeUniformUpdater : IUniformUpdater
{
	private readonly World world;

	public TimeUniformUpdater(World world)
	{
		this.world = world;
	}

	public void Update(string name, float deltaTime, Uniforms uniforms)
	{
		uniforms.UpdateValue<float>(name, t => t + world.Get<TimeScale>() * deltaTime);
	}
}
