namespace ShaderViewer.Components;

internal readonly record struct InputDelta(float Value)
{
	public InputDelta() : this(0.005f) { }

	public static implicit operator float(InputDelta inputDelta) => inputDelta.Value;
}
