using DefaultEcs;
using DefaultEcs.System;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ShaderViewer.Component;
using System;
using System.Collections.Generic;

namespace ShaderViewer.Systems
{
	internal sealed partial class DefaultUniformUpdateSystem : AEntitySetSystem<float>
	{
		public DefaultUniformUpdateSystem(World world, GameWindow window): base(world)
		{
			this.window = window;

			world.SubscribeComponentAdded((in Entity _, in Uniforms component) => FindDefaultUniforms(component));
			world.SubscribeComponentChanged((in Entity _, in Uniforms _, in Uniforms c) => FindDefaultUniforms(c));

			window.MouseDown += _ => button = GetButtonDown(window.MouseState);
			window.MouseUp += _ => button = GetButtonDown(window.MouseState);
		}

		private readonly GameWindow window;
		private readonly List<Action<float, Uniforms>> updaters = new();
		private int button;

		private void FindDefaultUniforms(in Uniforms uniforms)
		{
			updaters.Clear();
			foreach ((string name, object objValue) in uniforms.Pairs)
			{
				switch (name.ToLowerInvariant())
				{
					case "u_resolution":
					case "iresolution": updaters.Add((_ , u) => u.Set(name, window.ClientSize.ToVector2())); break;
					case "u_time":
					case "iglobaltime":
					case "itime": updaters.Add((deltaTime, u) => u.Update<float>(name, t => t + deltaTime)); break;
					case "u_mouse": updaters.Add((_, u) => u.Set(name, window.MousePosition)); break;
					case "imouse": updaters.Add((_, u) => u.Set(name, new Vector3(window.MousePosition.X, window.MousePosition.Y, button))); break;
				}
			}
		}

		static int GetButtonDown(MouseState m) => m[MouseButton.Left] ? 1 : (m[MouseButton.Right] ? 3 : (m[MouseButton.Middle]) ? 2 : 0);

		[Update]
		private void Update(float deltaTime, in Uniforms uniforms)
		{
			foreach(var updater in updaters)
			{
				updater(deltaTime, uniforms);
			}
		}
	}
}
