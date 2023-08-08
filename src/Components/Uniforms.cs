using System;
using System.Collections.Generic;
using System.Linq;

namespace ShaderViewer.Components;

internal class Uniforms
{
	//TODO: Try split each uniform into entity with name, value...
	public void Set(string name, object value)
	{
		Dictionary[name] = value;
	}

	public TType Get<TType>(string name)
	{
		if (!Dictionary.TryGetValue(name, out var obj)) throw new ArgumentException($"Uniform {name} not found.");
		if (obj is not TType) throw new ArgumentException($"Invalid type for Uniform {name}");
		return (TType)obj;
	}

	public void UpdateValue<TType>(string name, Func<TType, TType> updater) where TType : struct
	{
		var value = Get<TType>(name);
		Dictionary[name] = updater(value);
	}

	public IEnumerable<(string name, object value)> NameValue() => Dictionary.Select(p => (p.Key, p.Value));

	public readonly Dictionary<string, object> Dictionary = new();
}
