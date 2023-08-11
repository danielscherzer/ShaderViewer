using DefaultEcs;
using DefaultEcs.System;
using ImGuiNET;
using ShaderViewer.Component;
using System;
using System.Linq;

namespace ShaderViewer.System.Gui;

internal class MenuGuiSystem : ISystem<float>
{
	public MenuGuiSystem(World world)
	{
		this.world = world;
	}

	public bool IsEnabled { get; set; } = true;

	public void Dispose()
	{
	}

	public void Update(float deltaTime)
	{
		if (world.Get<ShowMenu>())
		{
			ImGui.BeginMainMenuBar();
			if (ImGui.BeginMenu("File"))
			{
				var recentFiles = world.Get<RecentFiles>();
				foreach (var fileName in recentFiles.Names.Reverse())
				{
					if (ImGui.MenuItem(fileName))
					{
						world.Set(new ShaderFile(fileName));
					}
				}
				//if(recentFiles.Names.Any()) ImGui.Separator();
				ImGui.EndMenu();
			}
			if (ImGui.BeginMenu("Window"))
			{
				float inputDelta = world.Get<InputDelta>();
				ImGui.DragFloat("Input delta", ref inputDelta, 0.005f, 0.005f, float.PositiveInfinity);
				world.Set(new InputDelta(inputDelta));

				float timeScale = world.Get<TimeScale>();
				ImGui.DragFloat("Time scale", ref timeScale, 0.1f, -100f, 100f);
				world.Set(new TimeScale(timeScale));

				var windowResolution = world.Get<WindowResolution>();
				var scaleFactor = windowResolution.ScaleFactor;
				ImGui.DragFloat("Resolution scale", ref scaleFactor, 0.005f, 0.1f, 4f);
				world.Set(windowResolution with { ScaleFactor = scaleFactor });
				
				ImGui.EndMenu();
			}
			ImGui.EndMainMenuBar();
		}
	}

	private readonly World world;
}