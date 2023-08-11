using DefaultEcs;
using DefaultEcs.System;
using OpenTK.Windowing.Desktop;
using ShaderViewer.Component;

namespace ShaderViewer.System.Uniform;

internal sealed partial class MouseUniformSystem : AEntitySetSystem<float>
{
	private readonly GameWindow window;

	public MouseUniformSystem(GameWindow window, World world) : base(world, CreateEntityContainer, true)
	{
		this.window = window;
	}

	[Update]
	private void Update(in Entity uniform, in Mouse _)
	{
		var scaleFactor = World.Get<WindowResolution>().ScaleFactor;
		uniform.Set(new UniformValue(scaleFactor * window.MousePosition));
	}
}