using DefaultEcs;
using DefaultEcs.System;
using ImGuiNET;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ShaderViewer.Component;
using System;

namespace ShaderViewer.System.Gui;

internal class CommandGuiSystem : ISystem<float>
{
	public CommandGuiSystem(GameWindow window, World world)
	{
		var query = world.GetEntities().With<Keys>().With<Action>();
		keyBindings = query.AsMap<Keys>();
		bindings = query.AsSet();
		void AddCommand(Keys keys, Func<string> text, Action action)
		{
			//var entity
			var entity = world.CreateEntity();
			entity.Set(keys);
			entity.Set(text);
			entity.Set(action);
		}

		AddCommand(Keys.Space,
			() => world.Get<TimeScale>() != 0f ? "Pause" : "Play",
			() => world.Set(new TimeScale(world.Get<TimeScale>() != 0f ? 0f : 1f)));
		AddCommand(Keys.LeftAlt, () => "Show Menu", () => world.Set(new ShowMenu(!world.Get<ShowMenu>())));
		AddCommand(Keys.Escape, () => "Exit", () => window.Close());

		window.KeyDown += args =>
		{
			if (keyBindings.TryGetEntity(args.Key, out var entity))
			{
				entity.Get<Action>()();
			}
		};
		this.world = world;
	}

	public bool IsEnabled { get; set; } = true;

	public void Dispose()
	{
		keyBindings.Dispose();
		bindings.Dispose();
	}

	public void Update(float deltaTime)
	{
		if (world.Get<ShowMenu>())
		{
			ImGui.BeginMainMenuBar();
			if (ImGui.BeginMenu("Window"))
			{
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
}