using DefaultEcs.System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using ShaderViewer.Component;
using Zenseless.OpenTK;

namespace ShaderViewer
{
	internal sealed partial class ShaderDrawSystem : AEntitySetSystem<float>
	{
		[Update]
		private static void Update(in ShaderProgram shaderProgram, in Uniforms uniforms)
		{
			shaderProgram.Bind();
			foreach ((string name, object objValue) in uniforms.NameValue)
			{
				var loc = GL.GetUniformLocation(shaderProgram.Handle, name);
				if (-1 != loc)
				{
					switch (objValue)
					{
						case float value: GL.ProgramUniform1(shaderProgram.Handle, loc, value); break;
						case Vector2 value: GL.ProgramUniform2(shaderProgram.Handle, loc, value); break;
						case Vector3 value: GL.ProgramUniform3(shaderProgram.Handle, loc, value); break;
						case Vector4 value: GL.ProgramUniform4(shaderProgram.Handle, loc, value); break;
					}
				}
			}
			GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
		}
	}
}
