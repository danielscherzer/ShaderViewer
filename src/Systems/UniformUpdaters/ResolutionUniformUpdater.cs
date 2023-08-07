using DefaultEcs;
using OpenTK.Mathematics;
using ShaderViewer.Components;
using ShaderViewer.Components.Shader;
using System.Collections.Generic;
using System.Linq;
using Zenseless.OpenTK;

namespace ShaderViewer.Systems.UniformUpdaters;

internal class ResolutionUniformUpdater : IUniformUpdater
{
	public ResolutionUniformUpdater(World world)
	{
		shaderResolution = world.Get<WindowResolution>().CalcShaderResolution().ToVector2();

		void ChangeResolution(WindowResolution windowResolution)
		{
			shaderResolution = windowResolution.CalcShaderResolution().ToVector2();
		}
		world.SubscribeWorldComponentAdded((World _, in WindowResolution resolution) => ChangeResolution(resolution));
		world.SubscribeWorldComponentChanged((World _, in WindowResolution _, in WindowResolution resolution) => ChangeResolution(resolution));
	}

	public bool ShouldBeActive(IEnumerable<string> currentUniformNames)
	{
		foreach (var name in names)
		{
			if (currentUniformNames.Contains(name))
			{
				currentName = name;
				return true;
			}

		}
		return false;
	}

	public void Update(float _, Uniforms uniforms)
	{
		uniforms.Set(currentName, shaderResolution);
	}

	private static readonly string[] names = new string[] { "u_resolution", "iResolution" };
	private string currentName = names[0];
	private Vector2 shaderResolution;
}
