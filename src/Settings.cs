﻿using DefaultEcs;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using ShaderViewer.Component;
using System.Linq;
using Zenseless.OpenTK;
using Zenseless.PersistentSettings;

namespace ShaderViewer;

internal static class Settings
{
	public static void Persist(GameWindow window, World world)
	{
		// set window postition/size relative to monitor size
		var info = Monitors.GetMonitorFromWindow(window);
		var monitorSize = new Vector2i(info.HorizontalResolution, info.VerticalResolution);
		window.Size = monitorSize / 2;
		window.Location = (monitorSize - window.Size) / 2;
		Vector2i Clamp(global::System.Numerics.Vector2 vec) => (Vector2i)Vector2.ComponentMin(monitorSize, vec.ToOpenTK());

		PersistentSettings settings = new();
		settings.AddFromGetterSetter("location", () => ((Vector2)window.Location).ToSystemNumerics(), v => window.Location = Clamp(v)); // Vector2 does not serialize, because of self referencing properties
		settings.AddFromGetterSetter("size", () => ((Vector2)window.Size).ToSystemNumerics(), v => window.Size = Clamp(v));
		settings.AddFromGetterSetter("inputDelta", () => world.Get<InputDelta>(), v => world.Set(v));
		settings.AddFromGetterSetter("recentFiles", () => world.Get<RecentFiles>(), v => world.Set(v));
		settings.AddFromGetterSetter("resolution", () => world.Get<WindowResolution>(), v => world.Set(v));
		settings.AddFromGetterSetter("alwaysOnTop", () => world.Get<AlwaysOnTop>(), v => world.Set(v));
		settings.Load();

		world.Set(new ShaderFile(world.Get<RecentFiles>().Names.LastOrDefault(string.Empty)));

		window.Closing += _ =>
		{
			window.WindowState = OpenTK.Windowing.Common.WindowState.Normal; // do not save window size in fullscreen or maximized
			settings.Store();
		};

		//var defaultFileName = Path.ChangeExtension(Assembly.GetCallingAssembly().Location, ".world");
		//ISerializer serializer = new TextSerializer(); // or BinarySerializer
		//using Stream stream = File.OpenRead(defaultFileName);
		//world = serializer.Deserialize(stream);

		//window.Closing += _ =>
		//{
		//	using Stream stream = File.Create(defaultFileName);
		//	serializer.Serialize(stream, world);
		//};
	}
}