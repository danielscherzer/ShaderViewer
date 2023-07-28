namespace ShaderViewer.Component;

internal readonly record struct SourceCode(string Value)
{
	public static implicit operator string(SourceCode sourceCode) => sourceCode.Value;
}
