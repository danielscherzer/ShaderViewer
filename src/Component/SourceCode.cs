namespace ShaderViewer.Component;

internal readonly record struct SourceCode(string Value)
{
	//public SourceCode() : this(string.Empty) { }
	public static implicit operator string(SourceCode sourceCode) => sourceCode.Value;
}
