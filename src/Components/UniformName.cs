namespace ShaderViewer.Components;

internal readonly record struct UniformName(string Name)
{
	public UniformName() : this(string.Empty) { }
}