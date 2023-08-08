using DefaultEcs;
using OpenTK.Mathematics;
using ShaderViewer.Components;
using System.Collections.Generic;
using System.Linq;
using Zenseless.OpenTK;

namespace ShaderViewer.Systems.Uniforms;

internal class ResolutionUniformUpdater : IUniformUpdater
{
	public ResolutionUniformUpdater(World world)
	{
		renderResolution = world.Get<WindowResolution>().CalcRenderResolution().ToVector2();

		void ChangeResolution(WindowResolution windowResolution)
		{
			renderResolution = windowResolution.CalcRenderResolution().ToVector2();
		}
		world.SubscribeWorldComponentAddedOrChanged((World _, in WindowResolution resolution) => ChangeResolution(resolution));
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

	public void Update(float _, Components.Uniforms uniforms)
	{
		uniforms.Set(currentName, renderResolution);
	}

	private static readonly string[] names = new string[] { "u_resolution", "iResolution" };
	private string currentName = names[0];
	private Vector2 renderResolution;
}
