using DefaultEcs;
using DefaultEcs.System;
using OpenTK.Windowing.Desktop;
using ShaderViewer;
using ShaderViewer.Systems;
using System;
using System.Linq;
using Zenseless.Resources;

using GameWindow window = new(GameWindowSettings.Default, NativeWindowSettings.Default);
using World world = new();
using SequentialSystem<float> systems = new(
	new DefaultUniformUpdateSystem(world, window),
	new ShaderDrawSystem(world),
	new LogGuiSystem(world),
	new UniformGuiSystem(world)
);
var resDir = new EmbeddedResourceDirectory(nameof(ShaderViewer) + ".content");

ShaderLoadSystem shaderLoadSystem = new(resDir, world);
shaderLoadSystem.Loaded += fileName => window.Title = fileName;

var fileName = Environment.GetCommandLineArgs().ElementAtOrDefault(1);
if(fileName is not null) shaderLoadSystem.LoadShaderFile(fileName);

window.FileDrop += args => shaderLoadSystem.LoadShaderFile(args.FileNames.First());
window.RenderFrame += args => systems.Update((float)args.Time);
Gui guiDrawSystem = new(window, resDir);
window.RenderFrame += _ => guiDrawSystem.Draw();
window.RenderFrame += _ => window.SwapBuffers();

PersistenceSystem persistenceSystem = new(window);
InputSystem inputSystem = new(window);

window.Run();