using DefaultEcs;
using DefaultEcs.System;
using ShaderViewer.Components.Shader;
using ShaderViewer.Systems.UniformUpdaters;
using System.Collections.Generic;

namespace ShaderViewer.Systems;

internal sealed partial class UniformUpdateSystem : AEntitySetSystem<float>
{
	public UniformUpdateSystem(World world,IReadOnlyDictionary<string, IUniformUpdater> updaters) : base(world, CreateEntityContainer, false)
	{
		void ActiveUpdaters(in Uniforms uniforms)
		{
			activeUpdaters.Clear();

			foreach ((string name, object objValue) in uniforms.NameValue())
			{
				if(updaters.TryGetValue(name.ToLowerInvariant(), out var updater))
				{
					activeUpdaters.Add((name, updater));
				}
			}
		}

		world.SubscribeComponentAdded((in Entity _, in Uniforms component) => ActiveUpdaters(component));
		world.SubscribeComponentChanged((in Entity _, in Uniforms _, in Uniforms c) => ActiveUpdaters(c));
	}

	private readonly List<(string name, IUniformUpdater updater)> activeUpdaters = new();

	[Update]
	private void Update(float deltaTime, in Uniforms uniforms)
	{
		foreach ((string name, IUniformUpdater updater) in activeUpdaters)
		{
			updater.Update(name, deltaTime, uniforms);
		}
	}
}
