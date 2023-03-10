using DefaultEcs.System;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using Zenseless.OpenTK;
using Zenseless.OpenTK.GUI;

namespace ShaderViewer.Systems
{
	internal class Gui : ISystem<float>
	{
		public Gui(GameWindow window, byte[] font)
		{
			_renderer = new ImGuiRenderer();
			_input = new ImGuiInput(window);

			Vector2i clientSize = window.ClientSize;
			_renderer.SetFont(font);
			ImGuiIOPtr io = ImGui.GetIO();
			io.FontGlobalScale = 1f;
			io.ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;
			io.ConfigWindowsResizeFromEdges = true;
			io.FontAllowUserScaling = true;

			var style = ImGui.GetStyle();
			style.FrameBorderSize = 1f;
			style.WindowBorderSize = 3f;
			style.WindowRounding = 10f;

			window.UpdateFrame += args => _input.Update(window.MouseState, window.KeyboardState, (float)args.Time);
			window.Resize += (window) => Resize(window.Width, window.Height);
			ImGui.NewFrame();
		}

		public bool IsEnabled { get; set; }

		internal static void Vec2Slider(string label, ref Vector2 v)
		{
			System.Numerics.Vector2 sysV = new(v.X, v.Y);
			if (ImGui.InputFloat2(label, ref sysV))
			//if (ImGui.SliderFloat2(label, ref sysV, -1f, 1f))
			{
				v = sysV.ToOpenTK();
			}
		}

		internal static void Resize(int width, int height)
		{
			ImGuiRenderer.WindowResized(width, height);
			GL.Viewport(0, 0, width, height);
		}

		public void Update(float state)
		{
			_renderer.Render();
			ImGui.NewFrame();
		}

		public void Dispose()
		{
			_renderer.Dispose();
		}

		private readonly ImGuiRenderer _renderer;
		private readonly ImGuiInput _input;
	}
}