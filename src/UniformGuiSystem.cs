﻿using DefaultEcs.System;
using ImGuiNET;
using OpenTK.Mathematics;
using ShaderViewer.Component;

namespace ShaderViewer
{
	internal sealed partial class UniformGuiSystem : AEntitySetSystem<float>
	{
		private float delta = 0.005f;

		[Update]
		private void Update(in Uniforms uniforms)
		{
			if (ImGui.Begin("Uniforms", ImGuiWindowFlags.AlwaysAutoResize))
			{
				ImGui.DragFloat("input delta", ref delta, 0.005f, 0.005f, float.PositiveInfinity);
				foreach ((string name, object objValue) in uniforms.NameValue)
				{
					switch (objValue)
					{
						case float value: ImGui.DragFloat(name, ref value, delta, float.NegativeInfinity, float.PositiveInfinity); uniforms.Set(name, value); break;
						case Vector2 value: Gui.Vec2Slider(name, ref value); uniforms.Set(name, value); break;
					}
				}
			}
			ImGui.End();
			//ImGui.ShowDemoWindow();
		}
	}
}
