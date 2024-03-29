﻿using DefaultEcs;
using System;
using System.Reactive.Disposables;

namespace ShaderViewer.System;

internal static class Extension
{
	public static IDisposable SubscribeEntityComponentAddedOrChanged<T>(this World world, EntityComponentAddedHandler<T> action)
	{
		return new CompositeDisposable(
			world.SubscribeEntityComponentAdded((in Entity entity, in T component) => action(entity, component)),
			world.SubscribeEntityComponentChanged((in Entity entity, in T _, in T component) => action(entity, component))
		);
	}

	public static IDisposable SubscribeWorldComponentAddedOrChanged<T>(this World world, WorldComponentAddedHandler<T> action)
	{
		return new CompositeDisposable(
			world.SubscribeWorldComponentAdded((World world, in T component) => action(world, component)),
			world.SubscribeWorldComponentChanged((World world, in T _, in T component) => action(world, component))
		);
	}
}
