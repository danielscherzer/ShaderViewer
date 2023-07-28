using DefaultEcs.System;
using ImGuiNET;
using ShaderViewer.Component;

namespace ShaderViewer.Systems
{
	internal sealed partial class LogGuiSystem : AEntitySetSystem<float>
	{
		[Update]
		private static void Update(in Log log)
		{
			ImGui.Begin("stats", ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoDecoration);
			ImGui.Text(log.Message);
			ImGui.End();
		}
	}
}