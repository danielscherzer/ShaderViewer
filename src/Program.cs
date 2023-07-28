using DefaultEcs;
using DefaultEcs.System;
using OpenTK.Windowing.Desktop;
using ShaderViewer.Component;
using ShaderViewer.Systems;
using System;
using System.Linq;
using Zenseless.OpenTK;

//TODO: using GameWindow window = new(GameWindowSettings.Default, NativeWindowSettings.Default);
using GameWindow window = new(GameWindowSettings.Default, ImmediateMode.NativeWindowSettings);
using World world = new();

world.SubscribeComponentAdded((in Entity entity, in ShaderFile shaderFile) => window.Title = shaderFile.Name);
world.SubscribeComponentChanged((in Entity entity, in ShaderFile _, in ShaderFile shaderFile) => window.Title = shaderFile.Name);

LoadShaderSourceSystem.Subscribe(world);

ParseUniformsSystem parseUniformsSystem = new(world);

var shaderFile = world.CreateEntity();

var fileName = Environment.GetCommandLineArgs().ElementAtOrDefault(1);
if (fileName is not null) shaderFile.Set(new ShaderFile(fileName));

window.FileDrop += args => shaderFile.Set(new ShaderFile(args.FileNames.First()));

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