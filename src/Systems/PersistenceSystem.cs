using DefaultEcs;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using ShaderViewer.Component;
using Zenseless.OpenTK;
using Zenseless.PersistentSettings;

namespace ShaderViewer.Systems;

internal static class PersistenceSystem
{
	public static void SubscribePersistenceSystem(this GameWindow window, World world)
	{
		// set window postition/size relative to monitor size
		var info = Monitors.GetMonitorFromWindow(window);
		var monitorSize = new Vector2i(info.HorizontalResolution, info.VerticalResolution);
		window.Size = monitorSize / 2;
		window.Location = (monitorSize - window.Size) / 2;
		Vector2i Clamp(System.Numerics.Vector2 vec) => (Vector2i)Vector2.ComponentMin(monitorSize, vec.ToOpenTK());

		PersistentSettings settings = new();
		settings.AddFromGetterSetter("location", () => ((Vector2)window.Location).ToSystemNumerics(), t => window.Location = Clamp(t));
		settings.AddFromGetterSetter("size", () => ((Vector2)window.Size).ToSystemNumerics(), t => window.Size = Clamp(t));
		settings.AddFromGetterSetter("inputDelta", () => world.Get<InputDelta>(), t => world.Set(t));
		settings.AddFromGetterSetter("recentFiles", () => world.Get<RecentFiles>(), t => world.Set(t));
		settings.Load();

		window.Closing += _ => settings.Store();


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