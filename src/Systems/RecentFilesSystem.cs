using DefaultEcs;
using ShaderViewer.Components;
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

		world.SubscribeWorldComponentAddedOrChanged((World _, in ShaderFile shaderFile) => Update(shaderFile.Name));
	}
}