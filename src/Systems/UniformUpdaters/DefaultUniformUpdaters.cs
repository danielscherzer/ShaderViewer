using DefaultEcs;
using OpenTK.Windowing.Desktop;
using System.Collections.Generic;

namespace ShaderViewer.Systems.UniformUpdaters;

internal static class DefaultUniformUpdaters
{
	internal static IReadOnlyDictionary<string, IUniformUpdater> Create(World world, GameWindow window)
	{
		var res = new ResolutionUniformUpdater(window);
		var time = new TimeUniformUpdater(world);
		var mouse = new MouseUniformUpdater(window);
		var imouse = new MouseButtonUniformUpdater(window);

		return new Dictionary<string, IUniformUpdater>()
		{
			["u_resolution"] = res,
			["iresolution"] = res,
			["u_time"] = time,
			["iglobaltime"] = time,
			["u_mouse"] = mouse,
			["imouse"] = imouse,
		};
		//case "icamposx":
		//case "icamposy":
		//case "icamposz":
		//case "icamrotx":
		//case "icamroty":
		//case "icamrotz": break;
		}
}
