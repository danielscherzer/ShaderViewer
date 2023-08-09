using DefaultEcs;
using DefaultEcs.System;
using GLSLhelper;
using ImGuiNET;
using OpenTK.Mathematics;
using ShaderViewer.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShaderViewer.Systems.Gui;

internal sealed partial class LogGuiSystem : AComponentSystem<float, Log>
{
	private ShaderLogLine[] lines = Array.Empty<ShaderLogLine>();

	public LogGuiSystem(World world): base(world)
	{
		world.SubscribeWorldComponentAddedOrChanged((World _, in Log log) =>
		{
			var parser = new ShaderLogParser(log.Message);
			lines = parser.Lines.ToArray();
		});
	}

	protected override void Update(float elapsedTime, ref Log _)
	{
		//TODO: Nicer output with warning/error categories
		if (ImGui.Begin("Shader Log", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar))
		{
			var flags = ImGuiTableFlags.Sortable | ImGuiTableFlags.Borders | ImGuiTableFlags.ScrollY | ImGuiTableFlags.RowBg;
			if (ImGui.BeginTable("table-shader-log", 3, flags))
			{
				ImGui.TableSetupColumn("Type", ImGuiTableColumnFlags.WidthFixed);
				ImGui.TableSetupColumn("Line", ImGuiTableColumnFlags.WidthFixed);
				ImGui.TableSetupColumn("Message");
				ImGui.TableSetupScrollFreeze(0, 1); // Make row always visible
				ImGui.TableHeadersRow();
				foreach (var line in lines)
				{
					ImGui.TableNextRow();
					ImGui.TableNextColumn();
					if(line.Type == MessageType.Error)
					{
						ImGui.TextColored(new System.Numerics.Vector4(1f, 0f, 0f, 1f), $"{line.Type}");
					}
					else ImGui.Text($"{line.Type}");
					ImGui.TableNextColumn();
					ImGui.Text($"{line.LineNumber ?? 0}");
					ImGui.TableNextColumn();
					ImGui.Text(line.Message);
				}
				ImGui.EndTable();
			}
			ImGui.End();
		}
	}
}