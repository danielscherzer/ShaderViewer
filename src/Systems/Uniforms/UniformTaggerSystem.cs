using DefaultEcs;
using ShaderViewer.Components;

namespace ShaderViewer.Systems.Gui;

internal static class UniformTaggerSystem
{
	public static void SubscribeUniformTaggerSystem(this World world)
	{
		//TODO: add registry
		static void Tag(Entity entity, string name)
		{
			switch (name)
			{
				case "iCamPos": entity.Set<CameraPos>(default); break;
				case "iCamRot": entity.Set<CameraRot>(default); break;
				case "u_resolution":
				case "iResolution": entity.Set<RenderResolution>(default); entity.Set<ReadOnly>(default); break;
				case "iTime":
				case "u_time":
				case "iGlobalTime": entity.Set<Time>(default); break;
				case "u_mouse": entity.Set<Mouse>(default); entity.Set<ReadOnly>(default); break;
				case "iMouse": entity.Set<IMouse>(default); entity.Set<ReadOnly>(default); break;
			}
		}
		world.SubscribeEntityComponentAddedOrChanged((in Entity entity, in UniformName uniformName) => Tag(entity, uniformName.Name));
	}
}
