using DefaultEcs.System;
using System;

internal class IsEnabledSystemDecorator<T>(Func<bool> isEnabled, ISystem<T> system) : ISystem<T>
{
	public ISystem<T> System { get; } = system;

	public bool IsEnabled { get => isEnabled(); set => throw new ArgumentException($"{nameof(IsEnabled)} should not be written to in a {nameof(IsEnabledSystemDecorator<T>)}"); }

	public void Dispose() => System.Dispose();

	public void Update(T state)
	{
		if (isEnabled()) System.Update(state);
	}
}