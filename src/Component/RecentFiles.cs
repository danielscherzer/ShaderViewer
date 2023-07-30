using System.Collections.Generic;
using System.Linq;

namespace ShaderViewer.Component;

internal readonly record struct RecentFiles(IEnumerable<string> Names)
{
	public RecentFiles() : this(Enumerable.Empty<string>()) { }

}