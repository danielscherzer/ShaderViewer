using DefaultEcs;
using DefaultEcs.System;
using ShaderViewer.Systems.Uniforms;
using System.Collections.Generic;
using System.Linq;

namespace ShaderViewer.Systems;

internal sealed partial class UniformUpdateSystem : AEntitySetSystem<float>
{
	public UniformUpdateSystem(World world) : base(world, CreateEntityContainer, false)
	{
		var defaultUniformUpdaters = world.GetEntities().With<IUniformUpdater>().AsMap<IUniformUpdater>();
		void ActiveUpdaters(Components.Shader.Uniforms uniforms)
		{
			activeUpdaters.Clear();
			activeUpdaters.AddRange(defaultUniformUpdaters.Keys.Where(updater => updater.ShouldBeActive(uniforms.Dictionary.Keys)));
		}

		world.SubscribeEntityComponentAddedOrChanged((in Entity _, in Components.Shader.Uniforms component) => ActiveUpdaters(component));
	}

	private readonly List<IUniformUpdater> activeUpdaters = new();

	[Update]
	private void Update(float deltaTime, in Components.Shader.Uniforms uniforms)
	{
		foreach (var updater in activeUpdaters)
		{
			updater.Update(deltaTime, uniforms);
		}
	}
}
