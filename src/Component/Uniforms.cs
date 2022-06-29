using System.Collections.Generic;
using System.Linq;

namespace ShaderViewer.Component
{
	internal class Uniforms
	{
		public void Set(string name, object value)
		{
			uniformValue[name] = value;
		}

		public IEnumerable<(string, object)> NameValue => uniformValue.Select(p => (p.Key, p.Value));

		private readonly Dictionary<string, object> uniformValue = new();
	}
}
