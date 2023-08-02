using OpenTK.Windowing.Desktop;
using ShaderViewer.Components.Shader;

namespace ShaderViewer.Systems.UniformUpdaters;

internal class MouseUniformUpdater : IUniformUpdater
{
	public MouseUniformUpdater(GameWindow window)
	{
		this.window = window;
	}

	public void Update(string name, float _, Uniforms uniforms)
	{
		uniforms.Set(name, window.MousePosition);
	}

	private readonly GameWindow window;
}
