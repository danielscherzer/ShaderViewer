using DefaultEcs;
using DefaultEcs.System;
using OpenTK.Windowing.Desktop;
using ShaderViewer.Component;
using ShaderViewer.Systems;
using System;
using System.Linq;
using Zenseless.OpenTK;

//using GameWindow window = new(GameWindowSettings.Default, NativeWindowSettings.Default); //TODO: core mode
using GameWindow window = new(GameWindowSettings.Default, ImmediateMode.NativeWindowSettings);
window.VSync = OpenTK.Windowing.Common.VSyncMode.On;

using World world = new();

world.SubscribeComponentAdded((in Entity _, in ShaderFile shaderFile) => window.Title = shaderFile.Name);
world.SubscribeComponentChanged((in Entity _, in ShaderFile _, in ShaderFile shaderFile) => window.Title = shaderFile.Name);

world.SubscribeRecentFilesSystem();
world.SubscribeLoadShaderSourceSystem();
world.SubscribeParseUniformsSystem();
window.SubscribePersistenceSystem(world);

var shader = world.CreateEntity();

var fileName = Environment.GetCommandLineArgs().ElementAtOrDefault(1);
if (fileName is not null) shader.Set(new ShaderFile(fileName));

window.FileDrop += args => shader.Set(new ShaderFile(args.FileNames.First()));

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

window.Run();