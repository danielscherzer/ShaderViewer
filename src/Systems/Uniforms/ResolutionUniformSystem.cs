using DefaultEcs;
using DefaultEcs.System;
using ShaderViewer.Components;

namespace ShaderViewer.Systems.Uniforms;

internal sealed partial class ResolutionUniformSystem : AEntitySetSystem<float>
{
	[Update]
	private void Update(in Entity uniform, in RenderResolution _)
	{
		var renderResolution = World.Get<WindowResolution>().CalcRenderResolution().ToVector2();
		uniform.Set(new UniformValue(renderResolution));
	}
}
