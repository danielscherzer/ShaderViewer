using ShaderViewer.Components.Shader;

namespace ShaderViewer.Systems.UniformUpdaters;

internal interface IUniformUpdater
{
	void Update(string name, float deltaTime, Uniforms uniforms);
}
