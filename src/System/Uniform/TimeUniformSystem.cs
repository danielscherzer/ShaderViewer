using DefaultEcs;
using DefaultEcs.System;
using ShaderViewer.Component;
using ShaderViewer.Component.Uniform;
using ShaderViewer.System.Gui;
using System.Reactive.Disposables;

namespace ShaderViewer.System.Uniform;

internal sealed partial class TimeUniformSystem : AEntitySetSystem<float>
{
	private readonly CompositeDisposable subscriptions;

	public TimeUniformSystem(World world) : base(world, CreateEntityContainer, true)
	{
		subscriptions = new CompositeDisposable(
			world.SubscribeUniform("iTime", entity => entity.Set<Time>(default)),
			world.SubscribeUniform("u_time", entity => entity.Set<Time>(default)),
			world.SubscribeUniform("iGlobalTime", entity => entity.Set<Time>(default))
		);
	}

	public override void Dispose()
	{
		subscriptions.Dispose();
		base.Dispose();
	}

	[Update]
	private void Update(float deltaTime, in Entity uniform, in Time _)
	{
		var time = uniform.Get<UniformValue>().Get<float>();
		uniform.Set(new UniformValue(time + (World.Get<TimeScale>() * deltaTime)));
	}
}
