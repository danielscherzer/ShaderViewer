using OpenTK.Mathematics;

namespace ShaderViewer.Component
{
	internal struct Resolution
	{
		public Resolution(int width, int height) : this()
		{
			Value = new Vector2(width, height);
		}

		public Vector2 Value { get; }
	}
}