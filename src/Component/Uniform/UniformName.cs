namespace ShaderViewer.Component.Uniform;

internal readonly record struct UniformName(string Name)
{
	public UniformName() : this(string.Empty) { }
}