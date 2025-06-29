using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ShaderViewer.Component;

internal readonly struct RecentFiles(IEnumerable<string> names)
{
	public RecentFiles() : this([]) { }

	public IEnumerable<string> Names { get; } = [.. names
			.Where(name => !string.IsNullOrWhiteSpace(name)) // no empty names
			.Reverse().Distinct().Reverse() // distinct elements, but from the end
			.TakeLast(20) // only last 20 elements
			.Where(File.Exists)]; // make a copy
}