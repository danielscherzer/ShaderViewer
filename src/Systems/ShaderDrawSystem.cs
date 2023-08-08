using DefaultEcs;
using DefaultEcs.System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using ShaderViewer.Components;
using Zenseless.OpenTK;

namespace ShaderViewer.Systems;

internal sealed partial class ShaderDrawSystem : AEntitySetSystem<float>
{
	private FrameBuffer frameBuffer;
	private WindowResolution windowResolution;
	private readonly ShaderProgram defaultShader;

	public ShaderDrawSystem(World world) : base(world, CreateEntityContainer, null, 0)
	{
		frameBuffer = new(true);
		windowResolution = world.Get<WindowResolution>();
		defaultShader = ShaderResources.CompileLink(ShaderResources.defaultFragmentSourceCode);
		void ChangeResolution(WindowResolution resolution)
		{
			windowResolution = resolution;
			frameBuffer.Dispose();
			frameBuffer = new(true);
			var res = resolution.CalcRenderResolution();
			frameBuffer.Attach(new Texture2D(res.X, res.Y), FramebufferAttachment.ColorAttachment0);
		}
		world.SubscribeWorldComponentAddedOrChanged((World _, in WindowResolution resolution) => ChangeResolution(resolution));
	}

	[Update]
	private void Update(in Entity entity, in ShaderProgram shaderProgram, in Components.Uniforms uniforms)
	{
		var shader = World.Has<Log>() ? defaultShader : shaderProgram;
		var localUniforms = uniforms;
		void Draw()
		{
			shader.Bind(); // because of this bind we can use GL.Uniform* commands

			foreach ((string name, object objValue) in localUniforms.NameValue())
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
		if (1f != windowResolution.ScaleFactor)
		{
			frameBuffer.Draw(Draw);
			var tex = frameBuffer.GetTexture(FramebufferAttachment.ColorAttachment0);
			GL.BlitNamedFramebuffer(frameBuffer.Handle, 0, 0, 0, tex.Width, tex.Height, 0, 0, windowResolution.Width, windowResolution.Height, ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Linear);
		}
		else
		{
			Draw();
		}
	}
}
