using DefaultEcs;
using ImGuiNET;

internal class LogDrawSystem
{
	private readonly World world;

	public LogDrawSystem(World world)
{
		this.world = world;
	}

	internal void Draw()
	{
		if(world.Has<string>())
		{
			var log = world.Get<string>();
			if (!string.IsNullOrEmpty(log))
			{
				ImGui.Begin("stats", ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoDecoration);
				ImGui.Text(log);
				ImGui.End();
			}
		}
	}
}