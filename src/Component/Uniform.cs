namespace ShaderViewer.Component
{
	internal struct Uniform
	{
		public Uniform(string name)
		{
			Name = name;
		}

		public string Name { get; }
	}
}