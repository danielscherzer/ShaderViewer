using System.Collections.Generic;
using System.Linq;

namespace ShaderViewer.Component;

internal readonly struct RecentFiles
{
	public RecentFiles() : this(Enumerable.Empty<string>()) { }

	public RecentFiles(IEnumerable<string> names)
	{
		Names = names
			.Where(name => !string.IsNullOrWhiteSpace(name)) // no empty names
			.Reverse().Distinct().Reverse() // distinct elements, but from the end
			.TakeLast(20) // only last 20 elements
			.ToArray(); // make a copy
	}

	public IEnumerable<string> Names { get; }
}