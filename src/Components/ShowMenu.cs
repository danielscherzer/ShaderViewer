namespace ShaderViewer.Components;

internal readonly record struct ShowMenu(bool Value)
{
	public ShowMenu() : this(true) { }

	public static implicit operator bool(ShowMenu showMenu) => showMenu.Value;
}
