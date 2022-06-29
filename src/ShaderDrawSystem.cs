using DefaultEcs;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using ShaderViewer.Component;
using Zenseless.OpenTK;
using Zenseless.Patterns;
using Zenseless.Resources;

namespace ShaderViewer
{
	internal class ShaderDrawSystem : Disposable
	{
		public ShaderDrawSystem(IResourceDirectory resourceDirectory, World world)
		{
			this.resourceDirectory = resourceDirectory;
			this.world = world;
			defaultFragmentSourceCode = resourceDirectory.Resource("checker.frag").OpenText();
			_shaderProgram = LoadShader(defaultFragmentSourceCode);
			_uniforms = world.GetEntities().With<Uniform>().AsSet();
		}

		internal void Draw()
		{
			if (!string.IsNullOrEmpty(log))
			{
				ImGui.Begin("stats", ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoDecoration); 
				ImGui.Text(log);
				ImGui.End();
			}
			_shaderProgram.Bind();
			var resolution = world.Get<Resolution>();
			_shaderProgram.Uniform("u_resolution", resolution.Value);
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
		}

		internal void Resize(int width, int height)
		{
			GL.Viewport(0, 0, width, height);
		}

		internal void SetShader(string shaderSource)
		{
			_shaderProgram.Dispose();
			try
			{
				_shaderProgram = LoadShader(shaderSource);
				log = "";
			}
			catch (ShaderException se)
			{
				log = se.Message;
				_shaderProgram = LoadShader(defaultFragmentSourceCode);
			}
		}

		protected override void DisposeResources() => _shaderProgram.Dispose();

		private readonly IResourceDirectory resourceDirectory;
		private readonly World world;
		private readonly string defaultFragmentSourceCode;
		private string log = "";
		private ShaderProgram _shaderProgram;
		private readonly EntitySet _uniforms;

		private ShaderProgram LoadShader(string shaderSource)
		{
			var vertex = (ShaderType.VertexShader, resourceDirectory.Resource("screenQuad.vert").OpenText());
			var fragment = (ShaderType.FragmentShader, shaderSource);
			return new ShaderProgram().CompileLink(vertex, fragment);
		}
	}
}
