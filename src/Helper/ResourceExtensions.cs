using OpenTK.Mathematics;
using System.IO;
using Zenseless.OpenTK;
using Zenseless.Resources;

namespace ShaderViewer.Helper
{
	public static class ResourceExtensions
	{
		public static System.Numerics.Vector2 FromOpenTK(this Vector2 vec) => new(vec.X, vec.Y);
		public static System.Numerics.Vector2 FromOpenTK(this Vector2i vec) => new(vec.X, vec.Y);
		public static Vector2 ToOpenTK(this System.Numerics.Vector2 vec) => new(vec.X, vec.Y);
		public static Vector3 ToOpenTK(this System.Numerics.Vector3 vec) => new(vec.X, vec.Y, vec.Z);
		public static Vector4 ToOpenTK(this System.Numerics.Vector4 vec) => new(vec.X, vec.Y, vec.Z, vec.W);

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
