using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common.Input;
using System;
using System.IO;
using System.Runtime.InteropServices;
using Zenseless.OpenTK;
using Zenseless.Resources;

namespace ShaderViewer.System;

internal static class Resources
{
	private static readonly ShortestMatchResourceDirectory resourceDirectory = new(new EmbeddedResourceDirectory());
	private static readonly (ShaderType, string) defaultVertexShader = (ShaderType.VertexShader, resourceDirectory.Resource("screenQuad.vert").AsString());
	public static readonly string defaultFragmentSourceCode = resourceDirectory.Resource("checker.frag").AsString();

	public static ShaderProgram CompileLink(string shaderSource)
	{
		var fragment = (ShaderType.FragmentShader, shaderSource);
		return new ShaderProgram().CompileLink(defaultVertexShader, fragment);
	}

	public static WindowIcon GetIcon()
	{
		using Stream stream = resourceDirectory.Resource("CornellBox.png").Open();
		using ImageMagick.MagickImage image = new(stream);
		IntPtr areaPointer = image.GetPixelsUnsafe().GetAreaPointer(0, 0, image.Width, image.Height);
		byte[] managedArray = new byte[image.Width * image.Height * 4];
		Marshal.Copy(areaPointer, managedArray, 0, managedArray.Length);
		var img = new Image(image.Width, image.Height, managedArray);
		return new WindowIcon(img);
	}
}
