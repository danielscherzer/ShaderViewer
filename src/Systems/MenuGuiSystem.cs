using DefaultEcs;
using DefaultEcs.System;
using ImGuiNET;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ShaderViewer.Component;
using System;

namespace ShaderViewer.Systems
{
	internal class MenuGuiSystem : ISystem<float>
	{
		public MenuGuiSystem(GameWindow window, World world)
		{
			var query = world.GetEntities().With<Keys>().With<Action>();
			keyBindings = query.AsMap<Keys>();
			bindings = query.AsSet();
			void AddKeyBinding(Keys keys, string text, Action action)
			{
				//var entity
				var entity = world.CreateEntity();
				entity.Set(keys);
				entity.Set(text);
				entity.Set(action);
			}
			AddKeyBinding(Keys.LeftAlt, "Show Menu", () => showMenu = !showMenu);
			AddKeyBinding(Keys.Escape, "Close", () => window.Close());

			window.KeyDown += args =>
			{
				if (keyBindings.TryGetEntity(args.Key, out var entity))
				{
					entity.Get<Action>()();
				}
			};
			keyBindings.Complete();
			bindings.Complete();
			this.world = world;
		}

		public bool IsEnabled { get; set; }

		public void Dispose()
		{
		}

		public void Update(float deltaTime)
		{
			if (showMenu)
			{
				ImGui.BeginMainMenuBar();
				if (ImGui.BeginMenu("File"))
				{
					foreach (var binding in bindings.GetEntities())
					{
						if (ImGui.MenuItem(binding.Get<string>(), binding.Get<Keys>().ToString()))
						{
							binding.Get<Action>()();
						}
					}
					ImGui.EndMenu();
				}
				if (ImGui.BeginMenu("Window"))
				{
					float inputDelta = world.Get<InputDelta>();
					ImGui.DragFloat("input delta", ref inputDelta, 0.005f, 0.005f, float.PositiveInfinity);
					world.Set(new InputDelta(inputDelta));
					ImGui.EndMenu();
				}
				ImGui.EndMainMenuBar();
			}
			//ImGui.ShowDemoWindow();
		}

		private readonly EntityMap<Keys> keyBindings;
		private readonly EntitySet bindings;
		private readonly World world;
		private bool showMenu = true; //TODO: move to world
	}
}