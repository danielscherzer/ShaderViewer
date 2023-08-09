using DefaultEcs;
using DefaultEcs.System;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ShaderViewer.Components;
using System;

namespace ShaderViewer.Systems.Uniforms;

internal class CameraUniformSystem : ISystem<float>
{
	public CameraUniformSystem(GameWindow window, World world, Func<bool> guiHasFocus)
	{
		World = world;

		cameraPos = world.GetEntities().With<CameraPos>().AsSet();
		cameraRot = world.GetEntities().With<CameraRot>().AsSet();

		this.guiHasFocus = guiHasFocus;
		this.window = window;
		window.KeyDown += args => StartMovement(args.Key);
		window.KeyUp += args => StopMovement(args.Key);
	}

	public bool IsEnabled { get; set; } = true;

	public World World { get; }

	private readonly EntitySet cameraPos;
	private readonly EntitySet cameraRot;

	public void Dispose()
	{
		cameraPos.Dispose();
		cameraRot.Dispose();
	}

	public void Update(float deltaTime)
	{
		if (!IsEnabled) return;
		if (guiHasFocus()) return;
		if (cameraPos.GetEntities().IsEmpty) return; //TODO: Make nicer
		if (cameraRot.GetEntities().IsEmpty) return;
		var posEntity = cameraPos.GetEntities()[0];
		var rotEntity = cameraRot.GetEntities()[0];
		var position = posEntity.Get<UniformValue>().Get<Vector3>();
		var rotation = rotEntity.Get<UniformValue>().Get<Vector3>();

		if (window.IsMouseButtonDown(MouseButton.Left))
		{
			var delta = Vector2.Divide(window.MouseState.Delta, window.ClientSize); // normalize
			rotation.Y += 15 * delta.X;
			rotation.X += -15 * delta.Y; // window y-axis goes downwards
		}
		FirstPersonCamera.Update(deltaTime * movement, ref position, rotation.Y, rotation.X);
		
		posEntity.Set(new UniformValue(position));
		rotEntity.Set(new UniformValue(rotation));
	}

	private Vector3 movement;

	private readonly GameWindow window;
	private readonly Func<bool> guiHasFocus;

	private void StartMovement(Keys key)
	{
		float inputDelta = World.Get<InputDelta>();
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
