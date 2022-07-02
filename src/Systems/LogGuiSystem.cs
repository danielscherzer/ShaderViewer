using DefaultEcs.System;
using ImGuiNET;

namespace ShaderViewer.Systems
{
	internal sealed partial class LogGuiSystem : AEntitySetSystem<float>
	{
		[Update]
		private static void Update(in string log)
		{
			if (!string.IsNullOrEmpty(log))
			{
				ImGui.Begin("stats", ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoDecoration);
				ImGui.Text(log);
				ImGui.End();
			}
		}
	}
}