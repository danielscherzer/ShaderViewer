using DefaultEcs;
using ShaderViewer.Component.Uniform;
using System;

namespace ShaderViewer.System.Gui;

internal static class Extension
{
	public static IDisposable SubscribeUniform(this World world, string uniformName, Action<Entity> entityAction)
	{
		return world.SubscribeEntityComponentAddedOrChanged((in Entity entity, in UniformName uniform) =>
		{
			if (uniformName == uniform.Name)
			{
				entityAction(entity);
			}
		});
	}
}
