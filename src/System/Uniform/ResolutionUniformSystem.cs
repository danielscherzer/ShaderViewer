using DefaultEcs;
using DefaultEcs.System;
using ShaderViewer.Component;
using ShaderViewer.Component.Uniform;
using ShaderViewer.System.Gui;
using System.Reactive.Disposables;

namespace ShaderViewer.System.Uniform;

internal sealed partial class ResolutionUniformSystem : AEntitySetSystem<float>
{
	private readonly CompositeDisposable subscriptions;

	public ResolutionUniformSystem(World world) : base(world, CreateEntityContainer, true)
	{
		static void Tag(Entity entity)
		{
			entity.Set<RenderResolution>(default);
			entity.Set<ReadOnly>(default);
		}

		subscriptions = new CompositeDisposable(
			world.SubscribeUniform("u_resolution", Tag),
			world.SubscribeUniform("iResolution", Tag)
		);
	}

	public override void Dispose()
	{
		subscriptions.Dispose();
		base.Dispose();
	}

	[Update]
	private void Update(in Entity uniform, in RenderResolution _)
	{
		var renderResolution = World.Get<WindowResolution>().CalcRenderResolution().ToVector2();
		uniform.Set(new UniformValue(renderResolution));
	}
}
