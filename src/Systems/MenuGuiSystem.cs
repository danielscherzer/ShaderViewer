using DefaultEcs;
using DefaultEcs.System;
using ImGuiNET;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;

namespace ShaderViewer.Systems
{
	internal class MenuGuiSystem : ISystem<float>
	{
		public MenuGuiSystem(GameWindow window, World world)
		{
			commandBindings.Add(Keys.Escape, ("Close", () => window.Close()));

			window.KeyDown += args =>
			{
				if(commandBindings.TryGetValue(args.Key, out var binding))
				{
					binding.action();
				}
			};
		}

		public bool IsEnabled { get; set; }

		public void Dispose()
		{
		}

		public void Update(float deltaTime)
		{
			if (ImGui.IsMouseClicked(ImGuiMouseButton.Right))
			{
				ImGui.OpenPopup("menu");
			}
			if (ImGui.BeginPopup("menu"))
			{
				foreach(var binding in commandBindings)
				{
					if (ImGui.MenuItem(binding.Value.name, binding.Key.ToString()))
					{
						binding.Value.action();
					}
				}
				ImGui.EndPopup();
			}
			//ImGui.ShowDemoWindow();
		}

		private readonly Dictionary<Keys, (string name, Action action)> commandBindings = new();
	}
}