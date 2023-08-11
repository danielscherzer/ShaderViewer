namespace ShaderViewer.Component;

internal readonly record struct ShaderFile(string Name)
{
	public ShaderFile() : this(string.Empty) { }
}