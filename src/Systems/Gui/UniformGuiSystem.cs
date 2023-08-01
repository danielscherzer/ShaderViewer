using DefaultEcs.System;
using ImGuiNET;
using OpenTK.Mathematics;
using ShaderViewer.Component;
using ShaderViewer.Components.Shader;
using Zenseless.OpenTK.GUI;

namespace ShaderViewer.Systems.Gui;

internal sealed partial class UniformGuiSystem : AEntitySetSystem<float>
{
	bool open = true;
	[Update]
	private void Update(in Uniforms uniforms)
	{
		float inputDelta = World.Get<InputDelta>();
		if (open && ImGui.Begin("Uniforms", ref open, ImGuiWindowFlags.AlwaysAutoResize))
		{
			foreach ((string name, object objValue) in uniforms.NameValue())
			{
				switch (objValue)
				{
					case float value: ImGui.DragFloat(name, ref value, inputDelta, float.NegativeInfinity, float.PositiveInfinity); uniforms.Set(name, value); break;
					case Vector2 value: ImGuiHelper.SliderFloat(name, ref value); uniforms.Set(name, value); break;
				}
			}
		}
		ImGui.End();
	}
}
