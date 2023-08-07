using DefaultEcs;
using DefaultEcs.System;
using ShaderViewer.Components.Shader;
using ShaderViewer.Systems.UniformUpdaters;
using System.Collections.Generic;
using System.Linq;

namespace ShaderViewer.Systems;

internal sealed partial class UniformUpdateSystem : AEntitySetSystem<float>
{
	public UniformUpdateSystem(World world) : base(world, CreateEntityContainer, false)
	{
		var defaultUniformUpdaters = world.GetEntities().With<IUniformUpdater>().AsMap<IUniformUpdater>();
		void ActiveUpdaters(Uniforms uniforms)
		{
			activeUpdaters.Clear();
			activeUpdaters.AddRange(defaultUniformUpdaters.Keys.Where(updater => updater.ShouldBeActive(uniforms.Dictionary.Keys)));
		}

		world.SubscribeEntityComponentAdded((in Entity _, in Uniforms component) => ActiveUpdaters(component));
		world.SubscribeEntityComponentChanged((in Entity _, in Uniforms _, in Uniforms c) => ActiveUpdaters(c));
	}

	private readonly List<IUniformUpdater> activeUpdaters = new();

	[Update]
	private void Update(float deltaTime, in Uniforms uniforms)
	{
		foreach (var updater in activeUpdaters)
		{
			updater.Update(deltaTime, uniforms);
		}
	}
}
