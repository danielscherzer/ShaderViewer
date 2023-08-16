using DefaultEcs;
using DefaultEcs.System;
using OpenTK.Windowing.Desktop;
using ShaderViewer.Component;
using ShaderViewer.Component.Uniform;
using ShaderViewer.System.Gui;
using System;

namespace ShaderViewer.System.Uniform;

internal sealed partial class MouseUniformSystem : AEntitySetSystem<float>
{
	private readonly GameWindow window;
	private readonly IDisposable subscription;

	public MouseUniformSystem(GameWindow window, World world) : base(world, CreateEntityContainer, true)
	{
		this.window = window;
		subscription = world.SubscribeUniform("u_mouse", entity => { entity.Set<Mouse>(default); entity.Set<ReadOnly>(default); });
	}

	public override void Dispose()
	{
		subscription.Dispose();
		base.Dispose();
	}

	[Update]
	private void Update(in Entity uniform, in Mouse _)
	{
		var scaleFactor = World.Get<WindowResolution>().ScaleFactor;
		uniform.Set(new UniformValue(scaleFactor * window.MousePosition));
	}
}