﻿using OpenTK.Mathematics;

namespace ShaderViewer.Components;

internal readonly record struct WindowResolution(int Width, int Height, float ScaleFactor)
{
	public WindowResolution() : this(512, 512, 1f) { }

	public Vector2i CalcShaderResolution() => (Vector2i)new Vector2(ScaleFactor * Width, ScaleFactor * Height);
}
