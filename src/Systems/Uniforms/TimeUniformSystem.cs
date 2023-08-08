using DefaultEcs;
using DefaultEcs.System;
using ShaderViewer.Components;

namespace ShaderViewer.Systems.Uniforms;

internal class TimeUniformSystem : ISystem<float>
{
	private readonly World world;
	private static readonly string[] names = new string[] { "iTime", "u_time", "iGlobalTime" };
	private string currentName = names[2];
	private Components.Shader.Uniforms uniforms;

	public bool IsEnabled { get; set; }

	public TimeUniformSystem(World world)
	{
		this.world = world;
		bool ShouldEnable(Components.Shader.Uniforms uniforms)
		{
			foreach (var name in names)
			{
				if (uniforms.Dictionary.ContainsKey(name))
				{
					currentName = name;
					this.uniforms = uniforms;
					return true;
				}

			}
			return false;
		}
		world.SubscribeEntityComponentAddedOrChanged((in Entity _, in Components.Shader.Uniforms uniforms) => IsEnabled = ShouldEnable(uniforms));
	}

	public void Update(float deltaTime)
	{
		uniforms.UpdateValue<float>(currentName, t => t + (world.Get<TimeScale>() * deltaTime));
	}

	public void Dispose()
	{
	}
}
