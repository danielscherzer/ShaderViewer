namespace ShaderViewer.Component
{
	internal readonly struct Log
	{
		public readonly string Message = "";

		public Log(string message)
		{
			Message = message;
		}

		public static implicit operator string(Log log) => log.Message;
	}
}
