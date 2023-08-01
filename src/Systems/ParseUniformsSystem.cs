using DefaultEcs;
using OpenTK.Mathematics;
using ShaderViewer.Components.Shader;
using System;

namespace ShaderViewer.Systems;

internal static class ParseUniformsSystem
{
	public static void SubscribeParseUniformsSystem(this World world)
	{
		world.SubscribeComponentAdded((in Entity entity, in SourceCode sourceCode) => Parse(entity, sourceCode));
		world.SubscribeComponentChanged((in Entity entity, in SourceCode _, in SourceCode sourceCode) => Parse(entity, sourceCode));
	}

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

	private static void Parse(Entity entity, string shaderSource)
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