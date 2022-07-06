namespace ShaderViewer.Component
{
	internal readonly struct InputDelta
	{
		public readonly float Value;

		public InputDelta()
		{
			Value = 0.005f;
		}

		public InputDelta(float value)
		{
			Value = value;
		}

		public static implicit operator float(InputDelta inputDelta) => inputDelta.Value;
	}
}
