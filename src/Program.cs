using DefaultEcs;
using DefaultEcs.System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using ShaderViewer;
using ShaderViewer.Component;
using ShaderViewer.System;
using ShaderViewer.System.Gui;
using ShaderViewer.System.Uniform;
using System;
using System.Linq;
using Zenseless.OpenTK;

//using GameWindow window = new(GameWindowSettings.Default, NativeWindowSettings.Default); //TODO: core mode
using GameWindow window = new(GameWindowSettings.Default, ImmediateMode.NativeWindowSettings);
window.VSync = OpenTK.Windowing.Common.VSyncMode.On;
window.Icon = Resources.GetIcon();

using World world = MyWorld.Create(window);

using SequentialSystem<float> systems = new(
	new TimeUniformSystem(world),
	new ResolutionUniformSystem(world),
	new MouseUniformSystem(window, world),
	new MouseButtonUniformSystem(window, world),
	new CameraUniformSystem(window, world, Gui.HasFocus),

	new ShaderLoadSystem(world),
	new ShaderDrawSystem(world),
	new IsEnabledSystemDecorator<float>(() => world.Get<ShowMenu>(), new SequentialSystem<float>(
		new MenuGuiSystem(world),
		new UniformGuiSystem(world),
		new CommandGuiSystem(window, world),
		new UpdateGuiSystem(window))),
	new LogGuiSystem(world),
	new Gui(window)
);

window.RenderFrame += _ => GL.Clear(ClearBufferMask.ColorBufferBit); // if no shader is loaded clears away closed menu remnants ...
window.RenderFrame += args => systems.Update((float)args.Time);
window.RenderFrame += _ => window.SwapBuffers();
window.Resize += args => world.Set(world.Get<WindowResolution>() with { Width = args.Width, Height = args.Height });

Settings.Persist(window, world); // call as late as possible because all subscriptions should be set-up (for instance uniform taggings)

var fileName = args.ElementAtOrDefault(1);
if (fileName is not null) world.Set(new ShaderFile(fileName));

window.Run();