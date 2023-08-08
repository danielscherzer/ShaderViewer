using DefaultEcs.System;
using ImGuiNET;
using ShaderViewer.Components;

namespace ShaderViewer.Systems.Gui;

internal sealed partial class LogGuiSystem : AEntitySetSystem<float>
{
	[Update]
	private static void Update(in Log log)
	{
		//TODO: Nicer output with warning/error categories
		if (ImGui.Begin("Messages", ImGuiWindowFlags.NoCollapse))
		{
			ImGui.Text(log.Message);
			ImGui.End();
		}
	}
}