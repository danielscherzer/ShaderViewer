using DefaultEcs;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using ShaderViewer.Component;
using System;
using System.IO;
using Zenseless.OpenTK;
using Zenseless.Resources;

namespace ShaderViewer
{
	internal class ShaderParseSystem
	{
		public ShaderParseSystem(IResourceDirectory resourceDirectory, World world)
		{
			this.resourceDirectory = resourceDirectory;
			this.world = world;

			defaultFragmentSourceCode = resourceDirectory.Resource("checker.frag").OpenText();
			world.Set(LoadShader(defaultFragmentSourceCode));
			Parse(defaultFragmentSourceCode);
		}

		public void Load(string fileName)
		{
			var shaderSource = File.ReadAllText(fileName);
			string dir = Path.GetDirectoryName(fileName) ?? "";
			//shaderSource = GLSLhelper.Transformation.ExpandIncludes(shaderSource, include => File.ReadAllText(Path.Combine(dir, include)));
			Parse(shaderSource);
		}

		private readonly IResourceDirectory resourceDirectory;
		private readonly World world;
		private readonly string defaultFragmentSourceCode;

		private static Type? GetType(string typeName)
		{
			return typeName switch
			{
				"float" => typeof(float),
				"vec2" => typeof(Vector2),
				"vec3" => typeof(Vector3),
				"vec4" => typeof(Vector4),
				"mat2" => typeof(Matrix2),
				"mat3" => typeof(Matrix3),
				"mat4" => typeof(Matrix4),
				_ => Type.GetType(typeName),
			};
		}

		private ShaderProgram LoadShader(string shaderSource)
		{
			var vertex = (ShaderType.VertexShader, resourceDirectory.Resource("screenQuad.vert").OpenText());
			var fragment = (ShaderType.FragmentShader, shaderSource);
			return new ShaderProgram().CompileLink(vertex, fragment);
		}

		private void Parse(string shaderSource)
		{
			world.Get<ShaderProgram>()?.Dispose();
			try
			{
				world.Set(LoadShader(shaderSource));
				world.Set("");
			}
			catch (ShaderException se)
			{
				world.Set(se.Message);
				world.Set(LoadShader(defaultFragmentSourceCode));
			}

			var uniformDeclaration = GLSLhelper.Extract.Uniforms(GLSLhelper.Transformation.RemoveComments(shaderSource));
			var uniforms = new Uniforms();
			foreach ((string typeName, string name) in uniformDeclaration)
			{
				var type = GetType(typeName);
				if (type is null) continue;
				var instance = Activator.CreateInstance(type);
				if (instance != null)
				{
					uniforms.Set(name, instance);
				}
			}
			world.Set(uniforms);
		}
	}
}