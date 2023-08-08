using System.Collections.Generic;

namespace ShaderViewer.Systems.Uniforms;

internal interface IUniformUpdater
{
	bool ShouldBeActive(IEnumerable<string> currentUniformNames);
	void Update(float deltaTime, Components.Shader.Uniforms uniforms);
}
