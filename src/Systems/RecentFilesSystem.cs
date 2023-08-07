using DefaultEcs;
using ShaderViewer.Components;
using ShaderViewer.Components.Shader;
using System.Linq;

namespace ShaderViewer.Systems;

internal static class RecentFilesSystem
{
	public static void SubscribeRecentFilesSystem(this World world)
	{
		void Update(string name)
		{
			var recentFiles = world.Get<RecentFiles>().Names;
			world.Set(new RecentFiles(recentFiles.Append(name)));
		}

		world.SubscribeEntityComponentAdded((in Entity entity, in ShaderFile shaderFile) => Update(shaderFile.Name));
		world.SubscribeEntityComponentChanged((in Entity entity, in ShaderFile _, in ShaderFile shaderFile) => Update(shaderFile.Name));
	}
}