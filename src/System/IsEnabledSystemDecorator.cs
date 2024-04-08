using DefaultEcs.System;
using System;

internal class IsEnabledSystemDecorator<T> : ISystem<T>
{

	public IsEnabledSystemDecorator(Func<bool> isEnabled, ISystem<T> system)
	{
		this.isEnabled = isEnabled;
		System = system;
	}

	public ISystem<T> System { get; }

	public bool IsEnabled { get => isEnabled(); set => throw new ArgumentException($"{nameof(IsEnabled)} should not be written to in a {nameof(IsEnabledSystemDecorator<T>)}"); }

	public void Dispose() => System.Dispose();

	public void Update(T state)
	{
		if (isEnabled()) System.Update(state);
	}

	private readonly Func<bool> isEnabled;
}