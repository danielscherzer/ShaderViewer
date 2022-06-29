using DefaultEcs;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ShaderViewer;
using ShaderViewer.Component;
using System.Linq;
using Zenseless.Resources;

var window = new GameWindow(GameWindowSettings.Default, NativeWindowSettings.Default);

using World world = new();

// set window postition/size relative to monitor size
var info = Monitors.GetMonitorFromWindow(window);
var monitorSize = new Vector2i(info.HorizontalResolution, info.VerticalResolution);
window.Size = monitorSize / 2;
window.Location = (monitorSize - window.Size) / 2;

var resDir = new EmbeddedResourceDirectory(nameof(ShaderViewer) + ".content");

ShaderParseSystem shaderParseSystem = new (resDir, world);
LogDrawSystem logDrawSystem = new(world);
ShaderDrawSystem shaderDrawSystem = new(world);
Gui guiDrawSystem = new(window, resDir);

window.FileDrop += args =>
{
	var fileName = args.FileNames.First();
	window.Title = fileName;
	shaderParseSystem.Load(fileName);
};

window.KeyDown += args =>
{
	switch (args.Key)
	{
		case Keys.Escape: window.Close(); break;
	}
};

window.RenderFrame += _ => world.Get<Uniforms>().Set("u_resolution", window.ClientSize.ToVector2());
window.RenderFrame += _ => shaderDrawSystem.Draw();
window.RenderFrame += _ => logDrawSystem.Draw();
window.RenderFrame += args => guiDrawSystem.Draw();
window.RenderFrame += _ => window.SwapBuffers();

window.Resize += (window) => guiDrawSystem.Resize(window.Width, window.Height);

window.Run();
