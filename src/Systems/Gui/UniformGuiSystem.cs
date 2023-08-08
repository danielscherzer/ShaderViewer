using DefaultEcs;
using DefaultEcs.System;
using ImGuiNET;
using OpenTK.Mathematics;
using ShaderViewer.Components;
using ShaderViewer.Systems.Uniforms;
using Zenseless.OpenTK.GUI;

namespace ShaderViewer.Systems.Gui;

internal sealed partial class UniformGuiSystem : AComponentSystem<float, Components.Uniforms>
{
	public UniformGuiSystem(World world) : base(world)
	{
	}

	protected override void Update(float _, ref Components.Uniforms uniforms)
	{
		if (!World.Get<ShowMenu>()) return;
		float inputDelta = World.Get<InputDelta>();
		ImGui.BeginMainMenuBar();
		if (ImGui.BeginMenu("Uniforms"))
		{
			foreach ((string name, object objValue) in uniforms.NameValue())
			{
				switch (objValue)
				{ //TODO: use read-only information to display other widget
					case float value: ImGui.DragFloat(name, ref value, inputDelta, float.NegativeInfinity, float.PositiveInfinity); uniforms.Set(name, value); break;
					case Vector2 value: ImGuiHelper.SliderFloat(name, ref value); uniforms.Set(name, value); break;
					case Vector3 value: ImGuiHelper.SliderFloat(name, ref value); uniforms.Set(name, value); break;
					case Vector4 value: ImGuiHelper.SliderFloat(name, ref value); uniforms.Set(name, value); break;
				}
			}
			if (World.Has<ShowCameraReset>())
			{
				ImGui.Separator();
				if (ImGui.MenuItem("Reset Camera"))
				{
					CameraUniformSystem.ResetCamera(uniforms);
				}
			}
			ImGui.EndMenu();
		}
		ImGui.EndMainMenuBar();
	}
}
