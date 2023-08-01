using DefaultEcs.System;
using ImGuiNET;
using ShaderViewer.Components.Shader;

namespace ShaderViewer.Systems.Gui;

internal sealed partial class LogGuiSystem : AEntitySetSystem<float>
{
	[Update]
	private static void Update(in Log log)
	{
		ImGui.Begin("Messages", ImGuiWindowFlags.AlwaysAutoResize);
		ImGui.Text(log.Message);
		ImGui.End();
	}
}