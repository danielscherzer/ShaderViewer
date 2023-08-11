using AutoUpdateViaGitHubRelease;
using DefaultEcs;
using DefaultEcs.System;
using ImGuiNET;
using OpenTK.Windowing.Desktop;
using ShaderViewer.Component;
using System.IO;
using System.Reflection;

namespace ShaderViewer.System.Gui;

internal class UpdateGuiSystem : ISystem<float>
{
	public UpdateGuiSystem(GameWindow window, World world)
	{
		this.window = window;
		this.world = world;
		update = new Update();
		var assembly = Assembly.GetExecutingAssembly();
		var name = assembly.GetName().Name ?? string.Empty;
		var version = assembly.GetName().Version;
		var tempDir = Path.Combine(Path.GetTempPath(), name);
		Directory.CreateDirectory(tempDir);
		update.CheckDownloadNewVersionAsync("danielScherzer", name, version, tempDir);
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
			if(update.Available)
			{
				if(ImGui.MenuItem("Update..."))
				{
					var destinationDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
					if (destinationDir != null)
					{
						update.StartInstall(destinationDir);
						window.Close();
					}
				}
			}
			ImGui.EndMainMenuBar();
		}
	}

	private readonly GameWindow window;
	private readonly World world;
	private readonly Update update;
}