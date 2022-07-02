using DefaultEcs;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ShaderViewer.Component;
using System;
using Zenseless.OpenTK;
using Zenseless.Resources;

namespace ShaderViewer
{
	internal class Gui
	{
		private readonly ImGuiController _controller;
		private readonly World world;
		private float delta = 0.005f;

		public Gui(GameWindow window, IResourceDirectory resourceDirectory, World world)
		{
			Vector2i clientSize = window.ClientSize;
			using var stream = resourceDirectory.Open("DroidSans.ttf");
			var buffer = new byte[stream.Length];
			stream.Read(buffer);
			_controller = new ImGuiController(clientSize.X, clientSize.Y, buffer);
			ImGuiIOPtr io = ImGui.GetIO();
			io.FontGlobalScale = 1f;
			io.ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;
			io.ConfigWindowsResizeFromEdges = true;
			io.FontAllowUserScaling = true;

			var style = ImGui.GetStyle();
			style.FrameBorderSize = 1f;
			style.WindowBorderSize = 3f;
			style.WindowRounding = 10f;

			window.MouseWheel += args => MouseScroll(args.Offset);
			window.TextInput += args => PressChar((char)args.Unicode);
			window.UpdateFrame += args => Update(window.MouseState, window.KeyboardState, (float)args.Time);
			this.world = world;
		}

		internal static void MouseScroll(Vector2 offset) => ImGuiController.MouseScroll(offset);

		internal void PressChar(char key) => _controller.PressChar(key);

		internal void Update(MouseState mouseState, KeyboardState keyboardState, float deltaTime)
		{
			_controller.Update(mouseState, keyboardState, deltaTime);
		}

		internal static void Vec2Slider(string label, ref Vector2 v)
		{
			System.Numerics.Vector2 sysV = new(v.X, v.Y);
			if(ImGui.InputFloat2(label, ref sysV))
			//if (ImGui.SliderFloat2(label, ref sysV, -1f, 1f))
			{
				v = sysV.ToOpenTK();
			}
		}

		internal static void Vec3Slider(string label, ref Vector3 v)
		{
			System.Numerics.Vector3 sysV = new(v.X, v.Y, v.Z);
			if (ImGui.SliderFloat3(label, ref sysV, -1f, 1f))
			{
				v = sysV.ToOpenTK();
			}
		}

		internal static void ColorEdit(string label, ref Vector3 color)
		{
			System.Numerics.Vector3 sysColor = new(color.X, color.Y, color.Z);
			if(ImGui.ColorEdit3(label, ref sysColor))
			{
				color = sysColor.ToOpenTK();
			}
		}

		internal static void Image(int handle)
		{
			var viewport = ImGui.GetMainViewport();
			var size = 0.2f * viewport.Size.X;
			ImGui.Image(new IntPtr(handle), new System.Numerics.Vector2(size), System.Numerics.Vector2.UnitY, System.Numerics.Vector2.UnitX);
		}

		internal void Draw()
		{
			DrawUniforms();
			_controller.Render();
		}

		private void DrawUniforms()
		{
			var uniforms = world.Get<Uniforms>();
			if (uniforms == null) return;
			if(ImGui.Begin("Uniforms", ImGuiWindowFlags.AlwaysAutoResize))
			{
				ImGui.InputFloat("input delta", ref delta);
				foreach ((string name, object objValue) in uniforms.NameValue)
				{
					switch (objValue)
					{
						case float value: ImGui.DragFloat(name, ref value, delta, float.NegativeInfinity, float.PositiveInfinity); uniforms.Set(name, value); break;
						case Vector2 value: Vec2Slider(name, ref value); uniforms.Set(name, value); break;
					}
				}
			}
			ImGui.End();
			//ImGui.ShowDemoWindow();
		}

		internal void Resize(int width, int height)
		{
			_controller.WindowResized(width, height);
			GL.Viewport(0, 0, width, height);
		}
	}
}