using System;
using System.Collections.Generic;
using System.Linq;

namespace ShaderViewer.Component;

internal class Uniforms
{
	public void Set(string name, object value)
	{
		Dictionary[name] = value;
	}

	public void RegisterUpdater<TType>(string name, Func<TType, TType> updater) where TType : struct
	{
		var obj = Dictionary[name];
		if (obj is not TType) throw new ArgumentException("Invalid type");
		TType value = (TType)obj;
		Dictionary[name] = updater(value);
	}

	public IEnumerable<(string name, object value)> NameValue() => Dictionary.Select(p => (p.Key, p.Value));

	public readonly Dictionary<string, object> Dictionary = new();
}
