using DefaultEcs;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using ShaderViewer.Component;
using Zenseless.OpenTK;
using Zenseless.PersistentSettings;

namespace ShaderViewer.Systems
{
	internal class PersistenceSystem
	{
		public PersistenceSystem(GameWindow window, World world)
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
			world.Set(new InputDelta());
			settings.AddFromGetterSetter("inputDelta", () => world.Get<InputDelta>(), t => world.Set(t));
			settings.Load();

			window.Closing += _ => settings.Store();
			//window.Closing += _ =>
			//{
			//	ISerializer serializer = new TextSerializer(); // or BinarySerializer
			//	using Stream stream = File.Create(@"d:\test.txt");
			//	serializer.Serialize(stream, world);
			//};
		}
	}
}