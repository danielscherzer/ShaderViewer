﻿using DefaultEcs;
using DefaultEcs.System;
using ImGuiNET;
using OpenTK.Mathematics;
using ShaderViewer.Components;
using Zenseless.OpenTK.GUI;

namespace ShaderViewer.Systems.Gui;

internal sealed partial class UniformGuiSystem : ISystem<float>
{
	public UniformGuiSystem(World world)
	{
		World = world;
		uniforms = world.GetEntities().With<UniformName>().AsSet();
		cameraPos = world.GetEntities().With<CameraPos>().AsSet();
		cameraRot = world.GetEntities().With<CameraRot>().AsSet();
	}

	public bool IsEnabled { get; set; }
	public World World { get; }

	private readonly EntitySet uniforms;
	private readonly EntitySet cameraPos;
	private readonly EntitySet cameraRot;

	public void Dispose()
	{
		uniforms.Dispose();
		cameraPos.Dispose();
		cameraRot.Dispose();
	}

	public void Update(float state)
	{
		if (!World.Get<ShowMenu>()) return;
		float inputDelta = World.Get<InputDelta>();
		ImGui.BeginMainMenuBar();
		if (ImGui.BeginMenu("Uniforms"))
		{
			foreach (var uniform in uniforms.GetEntities())
			{
				var name = uniform.Get<UniformName>().Name;
				if(uniform.Has<ReadOnly>())
				{
					//TODO: nicer formatting
					ImGui.Text(uniform.Get<UniformValue>().Value.ToString());
					ImGui.SameLine();
					ImGui.Text(name);
				}
				else
				{
					switch (uniform.Get<UniformValue>().Value)
					{
						case float value: ImGui.DragFloat(name, ref value, inputDelta, float.NegativeInfinity, float.PositiveInfinity); uniform.Set(new UniformValue(value)); break;
						case Vector2 value: ImGuiHelper.SliderFloat(name, ref value); uniform.Set(new UniformValue(value)); break;
						case Vector3 value: ImGuiHelper.SliderFloat(name, ref value); uniform.Set(new UniformValue(value)); break;
						case Vector4 value: ImGuiHelper.SliderFloat(name, ref value); uniform.Set(new UniformValue(value)); break;
					}
				}
			}
			if (!cameraPos.GetEntities().IsEmpty)
			{
				ImGui.Separator();
				if (ImGui.MenuItem("Reset Camera"))
				{
					var posEntity = cameraPos.GetEntities()[0];
					var rotEntity = cameraRot.GetEntities()[0];
					posEntity.Set(new UniformValue(Vector3.Zero));
					rotEntity.Set(new UniformValue(Vector3.Zero));
				}
			}
			ImGui.EndMenu();
		}
		ImGui.EndMainMenuBar();
	}
}
