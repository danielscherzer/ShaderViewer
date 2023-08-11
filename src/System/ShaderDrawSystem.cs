using DefaultEcs;
using DefaultEcs.System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using ShaderViewer.Component;
using Zenseless.OpenTK;

namespace ShaderViewer.System;

internal sealed partial class ShaderDrawSystem : AComponentSystem<float, ShaderProgram>
{
	private FrameBuffer frameBuffer;
	private WindowResolution windowResolution;
	private readonly ShaderProgram defaultShader;
	private readonly EntitySet uniforms;

	public ShaderDrawSystem(World world) : base(world)
	{
		frameBuffer = new(true);
		windowResolution = world.Get<WindowResolution>();
		defaultShader = ShaderResource.CompileLink(ShaderResource.defaultFragmentSourceCode);
		void ChangeResolution(WindowResolution resolution)
		{
			windowResolution = resolution;
			frameBuffer.Dispose();
			frameBuffer = new(true);
			var res = resolution.CalcRenderResolution();
			frameBuffer.Attach(new Texture2D(res.X, res.Y), FramebufferAttachment.ColorAttachment0);
		}
		world.SubscribeWorldComponentAddedOrChanged((World _, in WindowResolution resolution) => ChangeResolution(resolution));
		uniforms = world.GetEntities().With<UniformName>().AsSet();
	}

	protected override void Update(float _, ref ShaderProgram shaderProgram)
	{
		var shader = World.Has<Log>() ? defaultShader : shaderProgram;
		void Draw()
		{
			shader.Bind(); // because of this bind we can use GL.Uniform* commands

			foreach (var uniform in uniforms.GetEntities())
			{
				var name = uniform.Get<UniformName>().Name;
				var loc = GL.GetUniformLocation(shader.Handle, name);
				if (-1 != loc)
				{
					switch (uniform.Get<UniformValue>().Value)
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
