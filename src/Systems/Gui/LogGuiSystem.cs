using DefaultEcs;
using DefaultEcs.System;
using ImGuiNET;
using ShaderViewer.Components;

namespace ShaderViewer.Systems.Gui;

internal sealed partial class LogGuiSystem : AComponentSystem<float, Log>
{
	public LogGuiSystem(World world): base(world)
	{
	}

	protected override void Update(float elapsedTime, ref Log log)
	{
		//TODO: Nicer output with warning/error categories
		if (ImGui.Begin("Messages", ImGuiWindowFlags.NoCollapse))
		{
			ImGui.Text(log.Message);
			ImGui.End();
		}
	}
}