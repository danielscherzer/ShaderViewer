using DefaultEcs;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using ShaderViewer.Component;
using Zenseless.OpenTK;

namespace ShaderViewer
{
	internal class ShaderDrawSystem
	{
		public ShaderDrawSystem(World world)
		{
			this.world = world;
		}

		internal void Draw()
		{
			var _shaderProgram = world.Get<ShaderProgram>();
			var uniforms = world.Get<Uniforms>();
			_shaderProgram.Bind();
			foreach((string name , object objValue) in uniforms.NameValue)
			{
				var loc = GL.GetUniformLocation(_shaderProgram.Handle, name);
				if(-1 != loc)
				{
					switch(objValue)
					{
						case float value: GL.ProgramUniform1(_shaderProgram.Handle, loc, value); break;
						case Vector2 value: GL.ProgramUniform2(_shaderProgram.Handle, loc, value); break;
						case Vector3 value: GL.ProgramUniform3(_shaderProgram.Handle, loc, value); break;
						case Vector4 value: GL.ProgramUniform4(_shaderProgram.Handle, loc, value); break;
					}
				}
			}
			GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
		}

		private readonly World world;
	}
}
