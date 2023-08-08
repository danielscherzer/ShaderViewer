using DefaultEcs;
using OpenTK.Mathematics;
using ShaderViewer.Components;

namespace ShaderViewer.Systems.Uniforms;

internal class ResolutionUniformSystem : UniformsSystem
{
	public ResolutionUniformSystem(World world): base(world)
	{
		renderResolution = world.Get<WindowResolution>().CalcRenderResolution().ToVector2();

		void ChangeResolution(WindowResolution windowResolution)
		{
			renderResolution = windowResolution.CalcRenderResolution().ToVector2();
		}
		world.SubscribeWorldComponentAddedOrChanged((World _, in WindowResolution resolution) => ChangeResolution(resolution));
	}

	protected override bool ShouldEnable(Components.Uniforms uniforms)
	{
		foreach (var name in names)
		{
			if (uniforms.Dictionary.ContainsKey(name))
			{
				currentName = name;
				//this.uniforms = uniforms;
				return true;
			}

		}
		return false;
	}

	protected override void Update(float deltaTime, Components.Uniforms uniforms) => uniforms.Set(currentName, renderResolution);

	private static readonly string[] names = new string[] { "u_resolution", "iResolution" };
	private string currentName = names[0];
	private Vector2 renderResolution;
}
