namespace ShaderViewer.Component
{
	internal readonly struct SourceCode
	{
		public readonly string Value = "";

		public SourceCode(string sourceCode)
		{
			Value = sourceCode;
		}

		public static implicit operator string(SourceCode sourceCode) => sourceCode.Value;
	}
}
