using DefaultEcs;
using DefaultEcs.System;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ShaderViewer.Components;

namespace ShaderViewer.Systems.Uniforms;

internal sealed partial class MouseButtonUniformSystem : AEntitySetSystem<float>
{
	public MouseButtonUniformSystem(GameWindow window, World world) : base(world, CreateEntityContainer, true)
	{
		this.window = window;
		window.MouseDown += _ => button = GetButtonDown(window.MouseState);
		window.MouseUp += _ => button = GetButtonDown(window.MouseState);
	}

	[Update]
	private void Update(in Entity uniform, in Mouse _)
	{
		var scaleFactor = World.Get<WindowResolution>().ScaleFactor;
		var pos = scaleFactor * window.MousePosition;
		uniform.Set(new UniformValue(new Vector3(pos.X, pos.Y, button)));
	}

	private int button;
	private readonly GameWindow window;

	private static int GetButtonDown(MouseState m) => m[MouseButton.Left] ? 1 : (m[MouseButton.Right] ? 3 : m[MouseButton.Middle] ? 2 : 0);
}
