using ShaderViewer.Components.Shader;
using System.Collections.Generic;

namespace ShaderViewer.Systems.UniformUpdaters;

internal interface IUniformUpdater
{
	bool ShouldBeActive(IEnumerable<string> currentUniformNames);
	void Update(float deltaTime, Uniforms uniforms);
}
