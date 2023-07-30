using DefaultEcs;
using ShaderViewer.Component;
using System.Linq;

namespace ShaderViewer.Systems;

internal static class RecentFilesSystem
{
	public static void SubscribeRecentFilesSystem(this World world)
	{
		void Update(string name)
		{
			if (world.Has<RecentFiles>())
			{
				var recentFiles = world.Get<RecentFiles>();
				world.Set(new RecentFiles(recentFiles.Names.Append(name).Distinct()));
			}
			else
			{
				world.Set(new RecentFiles(new string[] { name }));
			}
		}

		world.SubscribeComponentAdded((in Entity entity, in ShaderFile shaderFile) => Update(shaderFile.Name));
		world.SubscribeComponentChanged((in Entity entity, in ShaderFile _, in ShaderFile shaderFile) => Update(shaderFile.Name));
	}
}