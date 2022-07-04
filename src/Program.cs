using DefaultEcs;
using DefaultEcs.System;
using OpenTK.Windowing.Desktop;
using ShaderViewer.Helper;
using ShaderViewer.Systems;
using System;
using System.Linq;
using Zenseless.Resources;

using GameWindow window = new(GameWindowSettings.Default, NativeWindowSettings.Default);
using World world = new();
//window.Context.MakeCurrent();

var resDir = new EmbeddedResourceDirectory(nameof(ShaderViewer) + ".content");

using SequentialSystem<float> systems = new(
	new DefaultUniformUpdateSystem(world, window),
	new ShaderLoadSystem(resDir, world),
	new ShaderDrawSystem(world),
	new MenuGuiSystem(window, world),
	new LogGuiSystem(world),
	new UniformGuiSystem(world)
);

LoadFileSystem loadFileSystem = new(world);
loadFileSystem.Loaded += fileName => window.Title = fileName;

ParseUniformsSystem parseUniformsSystem = new(world);

var fileName = Environment.GetCommandLineArgs().ElementAtOrDefault(1);
if (fileName is not null) loadFileSystem.LoadShaderFile(fileName);

window.FileDrop += args => loadFileSystem.LoadShaderFile(args.FileNames.First());
window.RenderFrame += args => systems.Update((float)args.Time);
Gui guiDrawSystem = new(window, resDir.Resource("DroidSans.ttf").AsByteArray());
window.RenderFrame += _ => guiDrawSystem.Draw();
window.RenderFrame += _ => window.SwapBuffers();

PersistenceSystem persistenceSystem = new(window);

window.Run();