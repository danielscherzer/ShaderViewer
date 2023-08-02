using DefaultEcs;
using DefaultEcs.System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using ShaderViewer.Components.Shader;
using Zenseless.OpenTK;

namespace ShaderViewer.Systems;

internal sealed partial class ShaderDrawSystem : AEntitySetSystem<float>
{
	private readonly ShaderProgram defaultShader;

	public ShaderDrawSystem(World world) : base(world, CreateEntityContainer, null, 0)
	{
		defaultShader = ShaderResources.CompileLink(ShaderResources.defaultFragmentSourceCode);
		// TODO: Render shader in given resolution
	}

	[Update]
	private void Update(in Entity entity, in ShaderProgram shaderProgram, in Uniforms uniforms)
	{
		var shader = entity.Has<Log>() ? defaultShader : shaderProgram;
		shader.Bind(); // because of this bind we can use GL.Uniform* commands
		foreach ((string name, object objValue) in uniforms.NameValue())
		{
			var loc = GL.GetUniformLocation(shader.Handle, name);
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
