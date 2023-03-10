using DefaultEcs;
using DefaultEcs.System;
using OpenTK.Windowing.Desktop;
using ShaderViewer.Systems;
using System;
using System.Linq;
using Zenseless.Resources;

using GameWindow window = new(GameWindowSettings.Default, NativeWindowSettings.Default);
using World world = new();

LoadFileSystem loadFileSystem = new(world);
loadFileSystem.Loaded += fileName => window.Title = fileName;

ParseUniformsSystem parseUniformsSystem = new(world);

var fileName = Environment.GetCommandLineArgs().ElementAtOrDefault(1);
if (fileName is not null) loadFileSystem.LoadShaderFile(fileName);

window.FileDrop += args => loadFileSystem.LoadShaderFile(args.FileNames.First());

var resDir = new ShortestMatchResourceDirectory(new EmbeddedResourceDirectory());

using SequentialSystem<float> systems = new(
	new DefaultUniformUpdateSystem(world, window),
	new ShaderLoadSystem(resDir, world),
	new ShaderDrawSystem(world),
	new MenuGuiSystem(window, world),
	new LogGuiSystem(world),
	new UniformGuiSystem(world),
	new Gui(window, resDir.Resource("DroidSans.ttf").AsByteArray())
);

window.RenderFrame += args => systems.Update((float)args.Time);
window.RenderFrame += _ => window.SwapBuffers();

PersistenceSystem persistenceSystem = new(window, world);

window.Run();