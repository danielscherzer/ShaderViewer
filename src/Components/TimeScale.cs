namespace ShaderViewer.Component;

internal readonly record struct TimeScale(float Value)
{
	public TimeScale() : this(1f) { }

	public static implicit operator float(TimeScale timeScale) => timeScale.Value;
}
