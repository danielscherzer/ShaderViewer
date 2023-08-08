using DefaultEcs;
using OpenTK.Windowing.Desktop;
using ShaderViewer.Components;

namespace ShaderViewer.Systems.Uniforms;

internal class MouseUniformSystem : UniformsSystem
{
	public MouseUniformSystem(GameWindow window, World world): base(world)
	{
		this.window = window;
		void ChangeScale(WindowResolution windowResolution)
		{
			scaleFactor = windowResolution.ScaleFactor;
		}
		world.SubscribeWorldComponentAddedOrChanged((World _, in WindowResolution resolution) => ChangeScale(resolution));
	}

	protected override bool ShouldEnable(Components.Uniforms uniforms) => uniforms.Dictionary.ContainsKey(name);

	protected override void Update(float deltaTime, Components.Uniforms uniforms)
	{
		uniforms.Set(name, scaleFactor * window.MousePosition);
	}

	protected readonly GameWindow window;
	protected float scaleFactor;
	private const string name = "u_mouse";
}
