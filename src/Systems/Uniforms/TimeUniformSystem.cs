using DefaultEcs;
using ShaderViewer.Components;

namespace ShaderViewer.Systems.Uniforms;

internal class TimeUniformSystem : UniformsSystem
{
	public TimeUniformSystem(World world): base(world)
	{
	}

	protected override bool ShouldEnable(Components.Uniforms uniforms)
	{
		foreach (var name in names)
		{
			if (uniforms.Dictionary.ContainsKey(name))
			{
				currentName = name;
				return true;
			}
		}
		return false;
	}

	protected override void Update(float deltaTime, Components.Uniforms uniforms)
	{
		uniforms.UpdateValue<float>(currentName, t => t + (World.Get<TimeScale>() * deltaTime));
	}

	private static readonly string[] names = new string[] { "iTime", "u_time", "iGlobalTime" };
	private string currentName = names[2];
}
