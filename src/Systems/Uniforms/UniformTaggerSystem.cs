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
				case "iCamPos": entity.Set(new CameraPos()); break;
				case "iCamRot": entity.Set(new CameraRot()); break;
				case "u_resolution":
				case "iResolution": entity.Set(new RenderResolution()); entity.Set(new ReadOnly()); break;
				case "iTime":
				case "u_time":
				case "iGlobalTime": entity.Set(new Time()); break;
				case "u_mouse": entity.Set(new Mouse()); entity.Set(new ReadOnly()); break;
				case "iMouse": entity.Set(new IMouse()); entity.Set(new ReadOnly()); break;
			}
		}
		world.SubscribeEntityComponentAddedOrChanged((in Entity entity, in UniformName uniformName) => Tag(entity, uniformName.Name));
	}
}
