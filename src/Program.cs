using DefaultEcs;
using DefaultEcs.System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using ShaderViewer.Components;
using ShaderViewer.Systems;
using ShaderViewer.Systems.Gui;
using ShaderViewer.Systems.Uniforms;
using System;
using System.Linq;
using Zenseless.OpenTK;

//using GameWindow window = new(GameWindowSettings.Default, NativeWindowSettings.Default); //TODO: core mode
using GameWindow window = new(GameWindowSettings.Default, ImmediateMode.NativeWindowSettings);
window.VSync = OpenTK.Windowing.Common.VSyncMode.On;

using World world = new();
world.Set(new TimeScale());
world.Set(new InputDelta());
world.Set(new RecentFiles());
world.Set(new ShowMenu());
world.Set(new WindowResolution());

world.SubscribeWorldComponentAddedOrChanged((World _, in ShaderFile shaderFile) => window.Title = shaderFile.Name);

//TODO: render shader not behind main menu bar
using SequentialSystem<float> systems = new(
	new TimeUniformSystem(world),
	new ResolutionUniformSystem(world),
	new MouseUniformSystem(window, world),
	new MouseButtonUniformSystem(window, world),
	new CameraUniformSystem(window, world, Gui.HasFocus),
	new ShaderLoadSystem(world),
	new ShaderDrawSystem(world),
	new MenuGuiSystem(window, world),
	new LogGuiSystem(world),
	new UniformGuiSystem(world),
	new Gui(window)
);

window.RenderFrame += _ => GL.Clear(ClearBufferMask.ColorBufferBit); // if no shader is loaded clears away closed menu remnants ...
window.RenderFrame += args => systems.Update((float)args.Time);
window.RenderFrame += _ => window.SwapBuffers();
window.Resize += args => world.Set(world.Get<WindowResolution>() with { Width = args.Width, Height = args.Height });

world.SubscribeRecentFilesSystem();
world.SubscribeReadShaderSourceSystem();
world.SubscribeParseUniformsSystem();
window.SubscribePersistenceSystem(world);

var fileName = Environment.GetCommandLineArgs().ElementAtOrDefault(1);
if (fileName is not null) world.Set(new ShaderFile(fileName));

window.FileDrop += args => world.Set(new ShaderFile(args.FileNames.First()));

window.Run();