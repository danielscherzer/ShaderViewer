using DefaultEcs;
using DefaultEcs.System;
using OpenTK.Windowing.Desktop;
using ShaderViewer.Systems;
using System;
using System.Linq;
using Zenseless.OpenTK;

//TODO: using GameWindow window = new(GameWindowSettings.Default, NativeWindowSettings.Default);
using GameWindow window = new(GameWindowSettings.Default, ImmediateMode.NativeWindowSettings);
using World world = new();

LoadFileSystem loadFileSystem = new(world);
loadFileSystem.Loaded += fileName => window.Title = fileName;

ParseUniformsSystem parseUniformsSystem = new(world);

var fileName = Environment.GetCommandLineArgs().ElementAtOrDefault(1);
//if (fileName is not null) loadFileSystem.LoadShaderFile(fileName);
if (fileName is not null) loadFileSystem.LoadShaderFile(fileName);

window.FileDrop += args => loadFileSystem.LoadShaderFile(args.FileNames.First());

using SequentialSystem<float> systems = new(
	new DefaultUniformUpdateSystem(world, window),
	new ShaderLoadSystem(window.Context, world),
	new ShaderDrawSystem(world),
	new MenuGuiSystem(window, world),
	new LogGuiSystem(world),
	new UniformGuiSystem(world),
	new Gui(window)
);

window.RenderFrame += args => systems.Update((float)args.Time);
window.RenderFrame += _ => window.SwapBuffers();

PersistenceSystem persistenceSystem = new(window, world);

window.Run();