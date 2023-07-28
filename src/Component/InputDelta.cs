namespace ShaderViewer.Component;

internal readonly record struct InputDelta(float Value)
{
	public static implicit operator float(InputDelta inputDelta) => inputDelta.Value;
}
//internal readonly struct InputDelta
//{
//	public readonly float Value;

//	public InputDelta()
//	{
//		Value = 0.005f;
//	}

//	public InputDelta(float value)
//	{
//		Value = value;
//	}

//	public static implicit operator float(InputDelta inputDelta) => inputDelta.Value;
//}
