using DefaultEcs;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ShaderViewer.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShaderViewer.Systems.Uniforms;

internal class CameraUniformUpdater : IUniformUpdater
{
	public CameraUniformUpdater(GameWindow window, World world, Func<bool> guiHasFocus)
	{
		this.guiHasFocus = guiHasFocus;
		this.window = window;
		this.world = world;
		window.KeyDown += args => StartMovement(args.Key);
		window.KeyUp += args => StopMovement(args.Key);
	}

	public static void ResetCamera(Components.Uniforms uniforms)
	{
		Set(uniforms, Vector3.Zero, 0f, 0f);
	}

	public static void Set(Components.Uniforms uniforms, Vector3 position, float heading, float tilt)
	{
		uniforms.Set(camPosX, position.X);
		uniforms.Set(camPosY, position.Y);
		uniforms.Set(camPosZ, position.Z);
		uniforms.Set(camRotX, tilt);
		uniforms.Set(camRotY, heading);
	}

	public bool ShouldBeActive(IEnumerable<string> currentUniformNames)
	{
		return currentUniformNames.Contains(camPosX);
	}

	public void Update(float deltaTime, Components.Uniforms uniforms)
	{
		if (guiHasFocus()) return;
		var x = uniforms.Get<float>(camPosX);
		var y = uniforms.Get<float>(camPosY);
		var z = uniforms.Get<float>(camPosZ);
		var tilt = uniforms.Get<float>(camRotX);
		var heading = uniforms.Get<float>(camRotY);
		Vector3 pos = new(x, y, z);
		if (window.IsMouseButtonDown(MouseButton.Left))
		{
			var delta = Vector2.Divide(window.MouseState.Delta, window.ClientSize); // normalize
			heading += 15 * delta.X;
			tilt += -15 * delta.Y; // window y-axis goes downwards
		}
		FirstPersonCamera.Update(deltaTime * movement, ref pos, heading, tilt);
		Set(uniforms, pos, heading, tilt);
	}

	private Vector3 movement;

	private const string camPosX = "iCamPosX";
	private const string camPosY = "iCamPosY";
	private const string camPosZ = "iCamPosZ";
	private const string camRotX = "iCamRotX";
	private const string camRotY = "iCamRotY";
	private readonly GameWindow window;
	private readonly World world;
	private readonly Func<bool> guiHasFocus;

	private void StartMovement(Keys key)
	{
		float inputDelta = world.Get<InputDelta>();
		var speed = 300f * inputDelta;
		switch (key)
		{
			case Keys.A: movement.X = -speed; break;
			case Keys.D: movement.X = speed; break;
			case Keys.Q: movement.Y = -speed; break;
			case Keys.E: movement.Y = speed; break;
			case Keys.W: movement.Z = speed; break;
			case Keys.S: movement.Z = -speed; break;
		}
	}

	private void StopMovement(Keys key)
	{
		switch (key)
		{
			case Keys.A: movement.X = 0f; break;
			case Keys.D: movement.X = 0f; break;
			case Keys.Q: movement.Y = 0f; break;
			case Keys.E: movement.Y = 0f; break;
			case Keys.W: movement.Z = 0f; break;
			case Keys.S: movement.Z = 0f; break;
		}
	}
}
