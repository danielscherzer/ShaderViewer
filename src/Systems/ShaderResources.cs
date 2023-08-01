using OpenTK.Graphics.OpenGL4;
using Zenseless.OpenTK;
using Zenseless.Resources;

namespace ShaderViewer.Systems;

internal static class ShaderResources
{
	private static readonly ShortestMatchResourceDirectory resourceDirectory = new(new EmbeddedResourceDirectory());
	private static readonly (ShaderType, string) defaultVertexShader = (ShaderType.VertexShader, resourceDirectory.Resource("screenQuad.vert").AsString());
	public static readonly string defaultFragmentSourceCode = resourceDirectory.Resource("checker.frag").AsString();

	public static ShaderProgram CompileLink(string shaderSource)
	{
		var fragment = (ShaderType.FragmentShader, shaderSource);
		return new ShaderProgram().CompileLink(defaultVertexShader, fragment);
	}
}
