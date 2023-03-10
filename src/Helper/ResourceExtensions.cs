using System.IO;
using Zenseless.OpenTK;
using Zenseless.Resources;

namespace ShaderViewer.Helper
{
	public static class ResourceExtensions
	{
		/// <summary>
		/// Load a texture out of the given embedded resource.
		/// </summary>
		/// <param name="name">The name of the resource that contains an image.</param>
		/// <returns>A Texture2D.</returns>
		public static Texture2D LoadTexture(this IResourceDirectory resourceDirectory, string name)
		{
			using Stream stream = resourceDirectory.Resource(name).Open();
			return Texture2DLoader.Load(stream);
		}
	}
}
