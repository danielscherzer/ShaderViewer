using DefaultEcs.System;
using ImGuiNET;
using OpenTK.Mathematics;
using ShaderViewer.Component;
using ShaderViewer.Helper;

namespace ShaderViewer.Systems
{
	internal sealed partial class UniformGuiSystem : AEntitySetSystem<float>
	{
		[Update]
		private void Update(in Uniforms uniforms)
		{
			float inputDelta = World.Get<InputDelta>();
			if (ImGui.Begin("Uniforms", ImGuiWindowFlags.AlwaysAutoResize))
			{
				ImGui.DragFloat("input delta", ref inputDelta, 0.005f, 0.005f, float.PositiveInfinity);
				World.Set(new InputDelta(inputDelta));
				foreach ((string name, object objValue) in uniforms.Pairs())
				{
					switch (objValue)
					{
						case float value: ImGui.DragFloat(name, ref value, inputDelta, float.NegativeInfinity, float.PositiveInfinity); uniforms.Set(name, value); break;
						case Vector2 value: Gui.Vec2Slider(name, ref value); uniforms.Set(name, value); break;
					}
				}
			}
			ImGui.End();
		}
	}
}
