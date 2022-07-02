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
	internal class ShaderLoadSystem
	{
		public ShaderLoadSystem(IResourceDirectory resourceDirectory, World world)
		{
			entity = world.CreateEntity();

			defaultVertexShader = (ShaderType.VertexShader, resourceDirectory.Resource("screenQuad.vert").OpenText());
			defaultFragmentSourceCode = resourceDirectory.Resource("checker.frag").OpenText();
			Load(defaultFragmentSourceCode);
			Parse(defaultFragmentSourceCode);
		}

		public void LoadShaderFile(string fileName)
		{
			var shaderSource = File.ReadAllText(fileName);
			string dir = Path.GetDirectoryName(fileName) ?? "";
			shaderSource = GLSLhelper.Transformation.ExpandIncludes(shaderSource, include => File.ReadAllText(Path.Combine(dir, include)));
			Load(shaderSource);
			Parse(shaderSource);
		}

		private readonly Entity entity;
		private readonly (ShaderType VertexShader, string) defaultVertexShader;
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

		private void Load(string shaderSource)
		{
			ShaderProgram LoadShader(string shaderSource)
			{
				var fragment = (ShaderType.FragmentShader, shaderSource);
				return new ShaderProgram().CompileLink(defaultVertexShader, fragment);
			}
			
			if(entity.Has<ShaderProgram>()) entity.Get<ShaderProgram>().Dispose();
			try
			{
				entity.Set(LoadShader(shaderSource));
				entity.Set("");
			}
			catch (ShaderException se)
			{
				entity.Set(se.Message);
				entity.Set(LoadShader(defaultFragmentSourceCode));
			}
		}

		private void Parse(string shaderSource)
		{
			var uniformDeclaration = GLSLhelper.Extract.Uniforms(GLSLhelper.Transformation.RemoveComments(shaderSource));
			Uniforms uniforms = new();
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
			entity.Set(uniforms);
		}
	}
}