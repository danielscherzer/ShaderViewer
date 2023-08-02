using DefaultEcs;
using DefaultEcs.System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using ShaderViewer.Component;
using ShaderViewer.Components.Shader;
using ShaderViewer.Systems;
using ShaderViewer.Systems.Gui;
using ShaderViewer.Systems.UniformUpdaters;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenseless.OpenTK;

//using GameWindow window = new(GameWindowSettings.Default, NativeWindowSettings.Default); //TODO: core mode
using GameWindow window = new(GameWindowSettings.Default, ImmediateMode.NativeWindowSettings);
window.VSync = OpenTK.Windowing.Common.VSyncMode.On;

using World world = new();
world.Set(new TimeScale());
world.Set(new InputDelta());
world.Set(new RecentFiles());

var shader = world.CreateEntity();
shader.Set(new ShaderFile(string.Empty)); // needed if no shader is set, so we find the entity

world.SubscribeComponentAdded((in Entity _, in ShaderFile shaderFile) => window.Title = shaderFile.Name);
world.SubscribeComponentChanged((in Entity _, in ShaderFile _, in ShaderFile shaderFile) => window.Title = shaderFile.Name);

world.SubscribeRecentFilesSystem();
world.SubscribeReadShaderSourceSystem();
world.SubscribeParseUniformsSystem();
window.SubscribePersistenceSystem(world);


var fileName = Environment.GetCommandLineArgs().ElementAtOrDefault(1);
if (fileName is not null) shader.Set(new ShaderFile(fileName));

window.FileDrop += args => shader.Set(new ShaderFile(args.FileNames.First()));

using SequentialSystem<float> systems = new(
	new UniformUpdateSystem(world, DefaultUniformUpdaters.Create(world, window)),
	new ShaderLoadSystem(window.Context, world),
	new ShaderDrawSystem(world),
	new MenuGuiSystem(window, world),
	new LogGuiSystem(world),
	new UniformGuiSystem(world),
	new Gui(window)
);

window.RenderFrame += _ => GL.Clear(ClearBufferMask.ColorBufferBit); // if no shader is loaded clears away closed menu remnants ...
window.RenderFrame += args => systems.Update((float)args.Time);
window.RenderFrame += _ => window.SwapBuffers();

window.Run();