using DefaultEcs;
using OpenTK.Windowing.Desktop;
using ShaderViewer.Components;
using System.Collections.Generic;
using System.Linq;

namespace ShaderViewer.Systems.Uniforms;

internal class MouseUniformUpdater : IUniformUpdater
{
	public MouseUniformUpdater(GameWindow window, World world)
	{
		this.window = window;
		void ChangeScale(WindowResolution windowResolution)
		{
			scaleFactor = windowResolution.ScaleFactor;
		}
		world.SubscribeWorldComponentAdded((World _, in WindowResolution resolution) => ChangeScale(resolution));
		world.SubscribeWorldComponentChanged((World _, in WindowResolution _, in WindowResolution resolution) => ChangeScale(resolution));
	}

	public virtual bool ShouldBeActive(IEnumerable<string> currentUniformNames) => currentUniformNames.Contains(name);

	public virtual void Update(float _, Components.Shader.Uniforms uniforms)
	{
		uniforms.Set(name, scaleFactor * window.MousePosition);
	}

	protected readonly GameWindow window;
	protected float scaleFactor;
	private const string name = "u_mouse";
}
