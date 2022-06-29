using DefaultEcs;
using ShaderViewer;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.IO;
using System.Linq;
using Zenseless.Resources;
using ShaderViewer.Component;
using GLSLhelper;

var window = new GameWindow(GameWindowSettings.Default, NativeWindowSettings.Default);

using World world = new();

// set window postition/size relative to monitor size
var info = Monitors.GetMonitorFromWindow(window);
var monitorSize = new Vector2i(info.HorizontalResolution, info.VerticalResolution);
window.Size = new Vector2i(2 * monitorSize.X / 5, 4 * monitorSize.Y / 5);
window.Location = (monitorSize - window.Size) / 2;

using ShaderDrawSystem view = new(new EmbeddedResourceDirectory(nameof(ShaderViewer) + ".content"), world);

window.FileDrop += args =>
{
	var fileName = args.FileNames.First();
	window.Title = fileName;
	string dir = Path.GetDirectoryName(fileName) ?? "";
	//view.SetShader(Transformation.ExpandIncludes(File.ReadAllText(fileName), include => File.ReadAllText(Path.Combine(dir, include))));
	view.SetShader(File.ReadAllText(fileName));
};

window.KeyDown += args =>
{
	switch (args.Key)
	{
		case Keys.Escape: window.Close(); break;
	}
};

Gui gui = new(window.ClientSize);

window.MouseWheel += args => Gui.MouseScroll(args.Offset);
window.TextInput += args => gui.PressChar((char)args.Unicode);

window.UpdateFrame += args => gui.Update(window.MouseState, window.KeyboardState, (float)args.Time);

window.RenderFrame += _ => view.Draw();
window.RenderFrame += args => gui.Draw();
window.RenderFrame += _ => window.SwapBuffers();

window.Resize += (window) => world.Set(new Resolution(window.Width, window.Height));
window.Resize += (window) => view.Resize(window.Width, window.Height);
window.Resize += (window) => gui.Resize(window.Width, window.Height);

window.Run();
