using DefaultEcs;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ShaderViewer.Component;
using ShaderViewer.System;
using System;
using System.IO;
using System.Linq;

namespace ShaderViewer
{
	internal class MyWorld
	{
		internal static World Create(GameWindow window)
		{
			World world = new();
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
			AddCommand(Keys.F11, () => "Fullscreen", () =>
			{
				window.WindowState = window.WindowState == WindowState.Fullscreen ? WindowState.Normal : WindowState.Fullscreen;
			});

			//TODO: world.SubscribeWorldComponentAddedOrChanged((World world, in TimeScale timeScale) => window.IsEventDriven = timeScale == 0f);

			world.SubscribeWorldComponentAddedOrChanged((World world, in ShaderFile shaderFile) =>
			{
				window.Title = Path.GetFileName(shaderFile.Name);
				world.Set(new RecentFiles(world.Get<RecentFiles>().Names.Append(shaderFile.Name)));
				//TODO: Check naming of some "systems"
				ReadShaderSourceSystem.Load(world, shaderFile.Name);
			});
			world.SubscribeWorldComponentAddedOrChanged((World world, in SourceCode sourceCode) => CreateUniformSystem.ParseShaderSource(world, sourceCode));

			window.FileDrop += args => world.Set(new ShaderFile(args.FileNames.First()));

			return world;
		}
	}
}
