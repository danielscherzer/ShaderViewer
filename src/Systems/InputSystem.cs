using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ShaderViewer.Systems
{
	internal class InputSystem
	{
		public InputSystem(GameWindow window)
		{
			window.KeyDown += args =>
			{
				switch (args.Key)
				{
					case Keys.Escape: window.Close(); break;
				}
			};
		}
	}
}