using DefaultEcs;
using OpenTK.Mathematics;
using ShaderViewer.Component;
using System;

namespace ShaderViewer.System;

internal static class ParseUniformSystem
{
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

	internal static void Parse(World world, string shaderSource)
	{
		foreach (var entity in world.GetEntities().With<UniformName>().AsSet().GetEntities())
		{
			entity.Dispose();
		}
		var uniformDeclaration = GLSLhelper.Extract.Uniforms(GLSLhelper.Transformation.RemoveComments(shaderSource));
		foreach ((string typeName, string name) in uniformDeclaration)
		{
			var type = GetType(typeName);
			if (type is null) continue;
			var instance = Activator.CreateInstance(type);
			if (instance != null)
			{
				var uniform = world.CreateEntity();
				uniform.Set(new UniformValue(instance));
				uniform.Set(new UniformName(name));
			}
		}
	}
}