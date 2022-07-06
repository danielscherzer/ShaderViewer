using DefaultEcs.System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using ShaderViewer.Component;
using Zenseless.OpenTK;

namespace ShaderViewer.Systems
{
	internal sealed partial class ShaderDrawSystem : AEntitySetSystem<float>
	{
		[Update]
		private static void Update(in ShaderProgram shaderProgram, in Uniforms uniforms)
		{
			shaderProgram.Bind(); // because of this bind we can use GL.Uniform* commands
			foreach ((string name, object objValue) in uniforms.Pairs())
			{
				var loc = GL.GetUniformLocation(shaderProgram.Handle, name);
				if (-1 != loc)
				{
					switch (objValue)
					{
						case float value: GL.Uniform1(loc, value); break;
						case Vector2 value: GL.Uniform2(loc, value); break;
						case Vector3 value: GL.Uniform3(loc, value); break;
						case Vector4 value: GL.Uniform4(loc, value); break;
					}
				}
			}
			GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
		}
	}
}
