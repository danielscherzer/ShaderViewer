using DefaultEcs;
using DefaultEcs.System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
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
using World world = new();
world.Set(new TimeScale());
world.Set(new InputDelta());
world.Set(new RecentFiles());
world.Set(new ShowMenu());
world.Set(new WindowResolution());
void AddCommand(Keys keys, Func<string> text, Action action, string menu = "")
{
	//TODO: Should we use a class for a command (some need construct and dispose functionality)
	var entity = world.CreateEntity();
	entity.Set(keys);
	entity.Set(text);
	entity.Set(action);
	entity.Set(string.IsNullOrWhiteSpace(menu) ? "Window" : menu);
}

AddCommand(Keys.Space,
	() => world.Get<TimeScale>() != 0f ? "Pause" : "Play",
	() => world.Set(new TimeScale(world.Get<TimeScale>() != 0f ? 0f : 1f)), "Uniforms");
AddCommand(Keys.LeftAlt, () => "Show Menu", () => world.Set(new ShowMenu(!world.Get<ShowMenu>())));
AddCommand(Keys.Escape, () => "Exit", () => window.Close(), "File");


world.SubscribeWorldComponentAddedOrChanged((World world, in ShaderFile shaderFile) =>
{
	window.Title = shaderFile.Name;
	world.Set(new RecentFiles(world.Get<RecentFiles>().Names.Append(shaderFile.Name)));
	ReadShaderSourceSystem.Load(world, shaderFile.Name);
});
world.SubscribeWorldComponentAddedOrChanged((World world, in SourceCode sourceCode) => ParseUniformSystem.Parse(world, sourceCode));
//TODO: Check naming of some systems
world.SubscribeUniformTaggerSystem();
window.SubscribePersistenceSystem(world);

//TODO: render shader not behind main menu bar
using SequentialSystem<float> systems = new(
	new TimeUniformSystem(world),
	new ResolutionUniformSystem(world),
	new MouseUniformSystem(window, world),
	new MouseButtonUniformSystem(window, world),
	new CameraUniformSystem(window, world, Gui.HasFocus),

	new ShaderLoadSystem(world),
	new ShaderDrawSystem(world),
	new MenuGuiSystem(world),
	new LogGuiSystem(world),
	new UniformGuiSystem(world),
	new CommandGuiSystem(window, world),
	new UpdateGuiSystem(window, world),
	new Gui(window)
);

window.RenderFrame += _ => GL.Clear(ClearBufferMask.ColorBufferBit); // if no shader is loaded clears away closed menu remnants ...
window.RenderFrame += args => systems.Update((float)args.Time);
window.RenderFrame += _ => window.SwapBuffers();
window.Resize += args => world.Set(world.Get<WindowResolution>() with { Width = args.Width, Height = args.Height });

var fileName = Environment.GetCommandLineArgs().ElementAtOrDefault(1);
if (fileName is not null) world.Set(new ShaderFile(fileName));

window.FileDrop += args => world.Set(new ShaderFile(args.FileNames.First()));

window.Run();