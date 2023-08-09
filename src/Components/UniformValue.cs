﻿using System;

namespace ShaderViewer.Components;

internal readonly record struct UniformValue(object Value)
{
	public readonly TType Get<TType>()
	{
		if (Value is not TType) throw new ArgumentException($"Invalid type for Uniform");
		return (TType)Value;
	}
}
