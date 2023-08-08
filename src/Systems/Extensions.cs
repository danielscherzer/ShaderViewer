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
	}
}
