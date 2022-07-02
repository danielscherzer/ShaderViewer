using DefaultEcs;
using OpenTK.Windowing.Desktop;
using ShaderViewer.Component;

internal class DefaultUniformSystem
{
	private World world;
	private GameWindow window;

	public DefaultUniformSystem(World world, GameWindow window)
	{
		this.world = world;
		this.window = window;

		world.SubscribeComponentAdded( (in Entity _, in Uniforms component) => UniformsChanged(component));
		world.SubscribeComponentChanged( (in Entity _, in Uniforms _, in Uniforms c) => UniformsChanged(c));

		window.RenderFrame += _ => world.Get<Uniforms>().Set("u_resolution", window.ClientSize.ToVector2());
	}

	private void UniformsChanged(in Uniforms uniforms)
	{
	}
}