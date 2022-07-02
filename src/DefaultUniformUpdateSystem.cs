using DefaultEcs;
using DefaultEcs.System;
using OpenTK.Windowing.Desktop;
using ShaderViewer.Component;

namespace ShaderViewer
{
	internal sealed partial class DefaultUniformUpdateSystem : AEntitySetSystem<float>
	{
		private readonly GameWindow window;

		public DefaultUniformUpdateSystem(World world, GameWindow window): base(world)
		{
			this.window = window;

			world.SubscribeComponentAdded((in Entity _, in Uniforms component) => UniformsChanged(component));
			world.SubscribeComponentChanged((in Entity _, in Uniforms _, in Uniforms c) => UniformsChanged(c));
		}

		private void UniformsChanged(in Uniforms uniforms)
		{
		}

		[Update]
		private void Update(in Uniforms uniforms)
		{
			uniforms.Set("u_resolution", window.ClientSize.ToVector2());
		}
	}
}
