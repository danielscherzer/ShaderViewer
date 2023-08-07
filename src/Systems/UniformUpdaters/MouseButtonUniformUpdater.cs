using DefaultEcs;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ShaderViewer.Components.Shader;
using System.Collections.Generic;
using System.Linq;

namespace ShaderViewer.Systems.UniformUpdaters;

internal class MouseButtonUniformUpdater : MouseUniformUpdater
{
	public MouseButtonUniformUpdater(GameWindow window, World world): base(window, world)
	{
		window.MouseDown += _ => button = GetButtonDown(window.MouseState);
		window.MouseUp += _ => button = GetButtonDown(window.MouseState);
	}

	public override bool ShouldBeActive(IEnumerable<string> currentUniformNames) => currentUniformNames.Contains(name);

	public override void Update(float _, Uniforms uniforms)
	{
		var pos = scaleFactor * window.MousePosition;
		uniforms.Set(name, new Vector3(pos.X, pos.Y, button));
	}

	private int button;
	private const string name = "imouse";

	private static int GetButtonDown(MouseState m) => m[MouseButton.Left] ? 1 : (m[MouseButton.Right] ? 3 : m[MouseButton.Middle] ? 2 : 0);
}
