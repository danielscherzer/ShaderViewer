using DefaultEcs;
using ShaderViewer.Components;
using ShaderViewer.Components.Shader;
using System.Collections.Generic;
using System.Linq;

namespace ShaderViewer.Systems.UniformUpdaters;

internal class TimeUniformUpdater : IUniformUpdater
{
	private readonly World world;
	private static readonly string[] names = new string[] { "iTime", "u_time", "iGlobalTime" };
	private string currentName = names[2];

	public TimeUniformUpdater(World world)
	{
		this.world = world;
	}

	public bool ShouldBeActive(IEnumerable<string> currentUniformNames)
	{
		foreach (var name in names)
		{
			if (currentUniformNames.Contains(name))
			{
				currentName = name; //TODO: avoid side-effect of ShouldBeActive
				return true;
			}

		}
		return false;
	}

	public void Update(float deltaTime, Uniforms uniforms)
	{
		uniforms.UpdateValue<float>(currentName, t => t + (world.Get<TimeScale>() * deltaTime));
	}
}
