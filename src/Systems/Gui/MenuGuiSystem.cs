using DefaultEcs;
using DefaultEcs.System;
using ImGuiNET;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ShaderViewer.Component;
using ShaderViewer.Components.Shader;
using System;
using System.Linq;

namespace ShaderViewer.Systems.Gui;

internal class MenuGuiSystem : ISystem<float>
{
	public MenuGuiSystem(GameWindow window, World world)
	{
		var query = world.GetEntities().With<Keys>().With<Action>();
		keyBindings = query.AsMap<Keys>();
		bindings = query.AsSet();
		void AddKeyBinding(Keys keys, Func<string> text, Action action)
		{
			//var entity
			var entity = world.CreateEntity();
			entity.Set(keys);
			entity.Set(text);
			entity.Set(action);
		}

		AddKeyBinding(Keys.Space,
			() => world.Get<TimeScale>() != 0f ? "Pause" : "Play",
			() => world.Set(new TimeScale(world.Get<TimeScale>() != 0f ? 0f : 1f)));
		AddKeyBinding(Keys.LeftAlt, () => "Show Menu", () => showMenu = !showMenu);
		AddKeyBinding(Keys.Escape, () => "Close Application", () => window.Close());

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
				var recentFiles = world.Get<RecentFiles>();
				foreach (var fileName in recentFiles.Names.Reverse())
				{
					if (ImGui.MenuItem(fileName))
					{
						var query = world.GetEntities().With<ShaderFile>().AsEnumerable();
						if (query.Any())
						{
							var entity = query.First();
							entity.Set(new ShaderFile(fileName));
						}
					}
				}
				//if(recentFiles.Names.Any()) ImGui.Separator();
				ImGui.EndMenu();
			}
			if (ImGui.BeginMenu("Window"))
			{
				float inputDelta = world.Get<InputDelta>();
				ImGui.DragFloat("input delta", ref inputDelta, 0.005f, 0.005f, float.PositiveInfinity);
				world.Set(new InputDelta(inputDelta));

				float timeScale = world.Get<TimeScale>();
				ImGui.DragFloat("time scale", ref timeScale, 0.1f, -100f, 100f);
				//var playing = timeScale != 0f;
				//if(ImGui.MenuItem(playing ? "Pause" : "Play"))
				//{
				//	timeScale = playing ? 0f : 1f;
				//}
				world.Set(new TimeScale(timeScale));

				foreach (var binding in bindings.GetEntities())
				{
					var text = binding.Get<Func<string>>()();
					if (ImGui.MenuItem(text, binding.Get<Keys>().ToString()))
					{
						binding.Get<Action>()();
					}
				}

				ImGui.EndMenu();
			}
			ImGui.EndMainMenuBar();
		}
	}

	private readonly EntityMap<Keys> keyBindings;
	private readonly EntitySet bindings;
	private readonly World world;
	private bool showMenu = true; //TODO: move to world
}