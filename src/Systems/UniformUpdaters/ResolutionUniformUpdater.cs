using OpenTK.Windowing.Desktop;
using ShaderViewer.Components.Shader;

namespace ShaderViewer.Systems.UniformUpdaters;

internal class ResolutionUniformUpdater : IUniformUpdater
{
	public ResolutionUniformUpdater(GameWindow window)
	{
		this.window = window;
	}

	public void Update(string name, float _, Uniforms uniforms)
	{
		uniforms.Set(name, window.ClientSize.ToVector2());
	}

	private readonly GameWindow window;
}
