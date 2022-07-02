using DefaultEcs;
using DefaultEcs.System;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ShaderViewer;
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

SequentialSystem<float> drawSystems = new(
	new DefaultUniformUpdateSystem(world, window),
	new ShaderDrawSystem(world),
	new LogGuiSystem(world),
	new UniformGuiSystem(world)
);

Gui guiDrawSystem = new Gui(window, resDir);

ShaderLoadSystem shaderLoadSystem = new(resDir, world);
window.FileDrop += args =>
{
	var fileName = args.FileNames.First();
	window.Title = fileName;
	shaderLoadSystem.LoadShaderFile(fileName);
};

window.KeyDown += args =>
{
	switch (args.Key)
	{
		case Keys.Escape: window.Close(); break;
	}
};

window.RenderFrame += args => drawSystems.Update((float)args.Time);
window.RenderFrame += _ => guiDrawSystem.Draw();
window.RenderFrame += _ => window.SwapBuffers();

window.Resize += (window) => guiDrawSystem.Resize(window.Width, window.Height);

window.Run();
