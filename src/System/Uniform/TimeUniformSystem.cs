using DefaultEcs;
using DefaultEcs.System;
using ShaderViewer.Component;
using ShaderViewer.Component.Uniform;

namespace ShaderViewer.System.Uniform;

internal sealed partial class TimeUniformSystem : AEntitySetSystem<float>
{
	[Update]
	private void Update(float deltaTime, in Entity uniform, in Time _)
	{
		var time = uniform.Get<UniformValue>().Get<float>();
		uniform.Set(new UniformValue(time + (World.Get<TimeScale>() * deltaTime)));
	}
}
