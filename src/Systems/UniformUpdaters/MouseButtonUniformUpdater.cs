using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ShaderViewer.Components.Shader;

namespace ShaderViewer.Systems.UniformUpdaters;

internal class MouseButtonUniformUpdater : IUniformUpdater
{
	public MouseButtonUniformUpdater(GameWindow window)
	{
		this.window = window;
		window.MouseDown += _ => button = GetButtonDown(window.MouseState);
		window.MouseUp += _ => button = GetButtonDown(window.MouseState);
	}

	public void Update(string name, float _, Uniforms uniforms)
	{
		uniforms.Set(name, new Vector3(window.MousePosition.X, window.MousePosition.Y, button));
	}

	private readonly GameWindow window;
	private int button;

	private static int GetButtonDown(MouseState m) => m[MouseButton.Left] ? 1 : (m[MouseButton.Right] ? 3 : m[MouseButton.Middle] ? 2 : 0);
}
