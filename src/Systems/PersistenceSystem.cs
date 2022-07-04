using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using ShaderViewer.Helper;
using Zenseless.PersistentSettings;

namespace ShaderViewer.Systems
{
	internal class PersistenceSystem
	{
		public PersistenceSystem(GameWindow window)
		{
			// set window postition/size relative to monitor size
			var info = Monitors.GetMonitorFromWindow(window);
			var monitorSize = new Vector2i(info.HorizontalResolution, info.VerticalResolution);
			window.Size = monitorSize / 2;
			window.Location = (monitorSize - window.Size) / 2;
			//window.WindowState = OpenTK.Windowing.Common.WindowState.Maximized;

			PersistentSettings settings = new();
			settings.AddFromGetterSetter("location", () => window.Location.FromOpenTK(), t => window.Location = t.ToOpenTKi());
			settings.AddFromGetterSetter("size", () => window.Size.FromOpenTK(), t => window.Size = t.ToOpenTKi());
			settings.Load();

			window.Closing += _ => settings.Store();
		}
	}
}