using DefaultEcs;
using DefaultEcs.System;
using GLSLhelper;
using ImGuiNET;
using ShaderViewer.Component;

namespace ShaderViewer.System.Gui;

internal sealed partial class LogGuiSystem : AComponentSystem<float, Log>
{
	private ShaderLogLine[] lines = [];

	public LogGuiSystem(World world) : base(world)
	{
		world.SubscribeWorldComponentAddedOrChanged((World _, in Log log) =>
		{
			var parser = new ShaderLogParser(log.Message);
			lines = [.. parser.Lines];
		});
	}

	protected override void Update(float elapsedTime, ref Log _)
	{
		//TODO: Nicer output with warning/error categories
		//TODO: If parsing was not possible do not write "Could not parse message..."
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
					if (line.Type == MessageType.Error)
					{
						ImGui.TextColored(new global::System.Numerics.Vector4(1f, 0f, 0f, 1f), $"{line.Type}");
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