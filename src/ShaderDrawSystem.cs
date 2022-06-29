using DefaultEcs;
using OpenTK.Graphics.OpenGL4;
using ShaderViewer.Component;
using Zenseless.OpenTK;

namespace ShaderViewer
{
	internal class ShaderDrawSystem
	{
		public ShaderDrawSystem(World world)
		{
			this.world = world;
			_uniforms = world.GetEntities().With<Uniform>().AsSet();
		}

		internal void Draw()
		{
			var _shaderProgram = world.Get<ShaderProgram>();
			_shaderProgram.Bind();
			foreach(var e in _uniforms.GetEntities())
			{
				var name = e.Get<Uniform>().Name;
			}
			var resolution = world.Get<Resolution>();
			_shaderProgram.Uniform("u_resolution", resolution.Value);
			GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
		}

		private readonly World world;
		private readonly EntitySet _uniforms;
	}
}
