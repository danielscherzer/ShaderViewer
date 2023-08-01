using DefaultEcs.System;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using Zenseless.OpenTK.GUI;

namespace ShaderViewer.Systems.Gui;

internal class Gui : ISystem<float>
{
    public Gui(GameWindow window)
    {
        _imgui = new ImGuiFacade(window);
        _imgui.LoadFontDroidSans(32f);

        Vector2i clientSize = window.ClientSize;
        ImGuiIOPtr io = ImGui.GetIO();
        io.FontGlobalScale = 1f;
        io.ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;
        io.ConfigWindowsResizeFromEdges = true;
        io.FontAllowUserScaling = true;

        var style = ImGui.GetStyle();
        style.FrameBorderSize = 1f;
        style.WindowBorderSize = 3f;
        style.WindowRounding = 10f;

        window.Resize += (window) => Resize(window.Width, window.Height);
        ImGui.NewFrame();
        this.window = window;
    }

    public bool IsEnabled { get; set; }

    internal static void Resize(int width, int height)
    {
        GL.Viewport(0, 0, width, height);
    }

    public void Update(float state)
    {
        _imgui.Render(window.ClientSize);
        ImGui.NewFrame();
    }

    public void Dispose()
    {
        _imgui.Dispose();
    }

    private readonly ImGuiFacade _imgui;
    private readonly GameWindow window;
}