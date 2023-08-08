using DefaultEcs;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ShaderViewer.Systems.Uniforms;

internal class MouseButtonUniformSystem : MouseUniformSystem
{
	public MouseButtonUniformSystem(GameWindow window, World world) : base(window, world)
	{
		window.MouseDown += _ => button = GetButtonDown(window.MouseState);
		window.MouseUp += _ => button = GetButtonDown(window.MouseState);
	}

	protected override bool ShouldEnable(Components.Uniforms uniforms) => uniforms.Dictionary.ContainsKey(name);

	protected override void Update(float _, Components.Uniforms uniforms)
	{
		var pos = scaleFactor * window.MousePosition;
		uniforms.Set(name, new Vector3(pos.X, pos.Y, button));
	}

	private int button;
	private const string name = "iMouse";

	private static int GetButtonDown(MouseState m) => m[MouseButton.Left] ? 1 : (m[MouseButton.Right] ? 3 : m[MouseButton.Middle] ? 2 : 0);
}
