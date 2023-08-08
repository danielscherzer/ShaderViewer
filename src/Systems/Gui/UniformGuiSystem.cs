using DefaultEcs;
using DefaultEcs.System;
using ImGuiNET;
using OpenTK.Mathematics;
using ShaderViewer.Components;
using ShaderViewer.Systems.Uniforms;
using System.Linq;
using Zenseless.OpenTK.GUI;

namespace ShaderViewer.Systems.Gui;

internal sealed partial class UniformGuiSystem : AEntitySetSystem<float>
{
	[Update]
	private void Update(in Components.Uniforms uniforms)
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
