namespace ShaderViewer.Component;

internal readonly record struct UniformName(string Name)
{
	public UniformName() : this(string.Empty) { }
}