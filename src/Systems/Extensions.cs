using DefaultEcs;

namespace ShaderViewer.Systems
{
	internal static class Extensions
	{
		public static void SubscribeEntityComponentAddedOrChanged<T>(this World world, EntityComponentAddedHandler<T> action)
		{
			world.SubscribeEntityComponentAdded((in Entity entity, in T component) => action(entity, component));
			world.SubscribeEntityComponentChanged((in Entity entity, in T _, in T component) => action(entity, component));
		}

		public static void SubscribeWorldComponentAddedOrChanged<T>(this World world, WorldComponentAddedHandler<T> action)
		{
			world.SubscribeWorldComponentAdded((World world, in T component) => action(world, component));
			world.SubscribeWorldComponentChanged((World world, in T _, in T component) => action(world, component));
		}
	}
}
