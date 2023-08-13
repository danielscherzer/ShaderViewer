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
		var query = world.GetEntities().With<Action>();
		keyBindings = query.With<Keys>().AsMap<Keys>();

		bindings = query.With<string>().AsMultiMap<string>();

		window.KeyDown += args =>
		{
			if (keyBindings.TryGetEntity(args.Key, out var entity))
			{
				entity.Get<Action>()();
			}
		};
	}

	public bool IsEnabled { get; set; } = true;

	public void Dispose()
	{
		keyBindings.Dispose();
		bindings.Dispose();
	}

	public void Update(float deltaTime)
	{
		ImGui.BeginMainMenuBar();
		foreach (var window in bindings.Keys)
		{
			if (ImGui.BeginMenu(window))
			{
				ImGui.Separator();
				foreach (var binding in bindings[window])
				{
					var text = binding.Get<Func<string>>()();
					var shortcut = binding.Has<Keys>() ? binding.Get<Keys>().ToString() : "";
					if (ImGui.MenuItem(text, shortcut))
					{
						binding.Get<Action>()();
					}
				}
				ImGui.EndMenu();
			}
		}
		ImGui.EndMainMenuBar();
	}

	private readonly EntityMap<Keys> keyBindings;
	private readonly EntityMultiMap<string> bindings;
}