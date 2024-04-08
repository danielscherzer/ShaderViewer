namespace ShaderViewer.Component;

internal readonly record struct AlwaysOnTop(bool Value)
{
	public AlwaysOnTop() : this(false) { }

	public static implicit operator bool(AlwaysOnTop instance) => instance.Value;
}
