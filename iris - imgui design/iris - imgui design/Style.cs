using ImGuiNET;
using System.Numerics;

public class ImGuiStyleConfig
{
    public bool _styleLoaded = false;
    public bool _fontLoaded = false;
    public bool _stopspammingconsole = false;
    public bool _designLoaded = false;
    public bool _colorsLoaded = false;
    public bool _textColorsLoaded = false;
    public bool _fontnotloaded = false;

    #region Style
    public void ApplyStyle()
    {


        if (_styleLoaded) return;
        if (_textColorsLoaded) return;
        if (_designLoaded) return;
        if (_colorsLoaded) return;
        if (_fontLoaded) return;
        var style = ImGui.GetStyle();
        /**/
        #region Design
        if (!_designLoaded)
        {
            style.WindowPadding = new Vector2(15, 15);
            style.WindowRounding = 5.0f;
            style.FramePadding = new Vector2(5, 5);
            style.FrameRounding = 4.0f;
            style.ItemSpacing = new Vector2(12, 8);
            style.ItemInnerSpacing = new Vector2(8, 6);
            style.IndentSpacing = 25.0f;
            style.ScrollbarSize = 15.0f;
            style.ScrollbarRounding = 9.0f;
            style.GrabMinSize = 5.0f;
            style.GrabRounding = 3.0f;
            _designLoaded = true;
            Console.WriteLine("Design Complete.");
        }
        #endregion


        #region Colors
        if (!_colorsLoaded)
        {
            style.Colors[(int)ImGuiCol.Text] = new Vector4(0.80f, 0.80f, 0.83f, 1.00f);
            style.Colors[(int)ImGuiCol.TextDisabled] = new Vector4(0.24f, 0.23f, 0.29f, 1.00f);
            style.Colors[(int)ImGuiCol.WindowBg] = new Vector4(0.06f, 0.05f, 0.07f, 1.00f);
            style.Colors[(int)ImGuiCol.ChildBg] = new Vector4(0.07f, 0.07f, 0.09f, 1.00f);
            style.Colors[(int)ImGuiCol.PopupBg] = new Vector4(0.07f, 0.07f, 0.09f, 1.00f);
            style.Colors[(int)ImGuiCol.Border] = new Vector4(0.80f, 0.80f, 0.83f, 0.88f);
            style.Colors[(int)ImGuiCol.BorderShadow] = new Vector4(0.92f, 0.91f, 0.88f, 0.00f);
            style.Colors[(int)ImGuiCol.FrameBg] = new Vector4(0.10f, 0.09f, 0.12f, 1.00f);
            style.Colors[(int)ImGuiCol.FrameBgHovered] = new Vector4(0.24f, 0.23f, 0.29f, 1.00f);
            style.Colors[(int)ImGuiCol.FrameBgActive] = new Vector4(0.56f, 0.56f, 0.58f, 1.00f);
            style.Colors[(int)ImGuiCol.TitleBg] = new Vector4(0.10f, 0.09f, 0.12f, 1.00f);
            style.Colors[(int)ImGuiCol.TitleBgCollapsed] = new Vector4(1.00f, 0.98f, 0.95f, 0.75f);
            style.Colors[(int)ImGuiCol.TitleBgActive] = new Vector4(0.07f, 0.07f, 0.09f, 1.00f);
            style.Colors[(int)ImGuiCol.MenuBarBg] = new Vector4(0.10f, 0.09f, 0.12f, 1.00f);
            style.Colors[(int)ImGuiCol.ScrollbarBg] = new Vector4(0.10f, 0.09f, 0.12f, 1.00f);
            style.Colors[(int)ImGuiCol.ScrollbarGrab] = new Vector4(0.80f, 0.80f, 0.83f, 0.31f);
            style.Colors[(int)ImGuiCol.ScrollbarGrabHovered] = new Vector4(0.56f, 0.56f, 0.58f, 1.00f);
            style.Colors[(int)ImGuiCol.ScrollbarGrabActive] = new Vector4(0.06f, 0.05f, 0.07f, 1.00f);
            style.Colors[(int)ImGuiCol.CheckMark] = new Vector4(0.80f, 0.80f, 0.83f, 0.31f);
            style.Colors[(int)ImGuiCol.SliderGrab] = new Vector4(0.80f, 0.80f, 0.83f, 0.31f);
            style.Colors[(int)ImGuiCol.SliderGrabActive] = new Vector4(0.06f, 0.05f, 0.07f, 1.00f);
            style.Colors[(int)ImGuiCol.Button] = new Vector4(0.10f, 0.09f, 0.12f, 1.00f);
            style.Colors[(int)ImGuiCol.ButtonHovered] = new Vector4(0.24f, 0.23f, 0.29f, 1.00f);
            style.Colors[(int)ImGuiCol.ButtonActive] = new Vector4(0.56f, 0.56f, 0.58f, 1.00f);
            style.Colors[(int)ImGuiCol.Tab] = new Vector4(0.501960813999176f, 0.3019607961177826f, 1.0f, 0.5490196347236633f);
            style.Colors[(int)ImGuiCol.TabHovered] = new Vector4(0.24f, 0.24f, 0.24f, 0.5490196347236633f);
            style.Colors[(int)ImGuiCol.TabSelected] = new Vector4(0.501960813999176f, 0.3019607961177826f, 1.0f, 0.5490196347236633f);
            style.Colors[(int)ImGuiCol.Header] = new Vector4(0.10f, 0.09f, 0.12f, 1.00f);
            style.Colors[(int)ImGuiCol.HeaderHovered] = new Vector4(0.56f, 0.56f, 0.58f, 1.00f);
            style.Colors[(int)ImGuiCol.HeaderActive] = new Vector4(0.06f, 0.05f, 0.07f, 1.00f);
            style.Colors[(int)ImGuiCol.ResizeGrip] = new Vector4(0.00f, 0.00f, 0.00f, 0.00f);
            style.Colors[(int)ImGuiCol.ResizeGripHovered] = new Vector4(0.56f, 0.56f, 0.58f, 1.00f);
            style.Colors[(int)ImGuiCol.ResizeGripActive] = new Vector4(0.06f, 0.05f, 0.07f, 1.00f);
            style.Colors[(int)ImGuiCol.PlotLines] = new Vector4(0.40f, 0.39f, 0.38f, 0.63f);
            style.Colors[(int)ImGuiCol.PlotLinesHovered] = new Vector4(0.25f, 1.00f, 0.00f, 1.00f);
            style.Colors[(int)ImGuiCol.PlotHistogram] = new Vector4(0.40f, 0.39f, 0.38f, 0.63f);
            style.Colors[(int)ImGuiCol.PlotHistogramHovered] = new Vector4(0.25f, 1.00f, 0.00f, 1.00f);
            style.Colors[(int)ImGuiCol.TextSelectedBg] = new Vector4(0.25f, 1.00f, 0.00f, 0.43f);
            style.Colors[(int)ImGuiCol.ModalWindowDimBg] = new Vector4(1.00f, 0.98f, 0.95f, 0.73f);

            _colorsLoaded = true;
            Console.WriteLine("Colors Complete.");
        }
        #endregion
        _styleLoaded = true;
        Console.WriteLine("Style Complete.");

    }
    #endregion
    // CAN THIS FUCKING FONT WORK FOR ONCE? PLEASE IF SOMEBODY KNOWS C# IMGUI FONT LOADING PLEASE DM ME ON DISCORD @regularlunar
    #region Font
    public void LoadFont()
    {
        if (_fontLoaded) return;

        ImGuiIOPtr io = ImGui.GetIO();
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string fontPath = Path.Combine(baseDirectory, "Font", "Ruda-Bold.ttf");

        if (!_stopspammingconsole)
        {
            Console.WriteLine($"Base directory: {baseDirectory}");
            Console.WriteLine($"Font path: {fontPath}");
            _stopspammingconsole = true;
        }

        if (File.Exists(fontPath))
        {
            ImFontPtr font = io.Fonts.AddFontFromFileTTF(fontPath, 16.0f);
            io.Fonts.Build();
            ImGui.PushFont(font);
            _fontLoaded = true;
            if (!_stopspammingconsole)
            {
                Console.WriteLine($"Font loaded from: {fontPath}");
            }
        }
        else
        {
            if (!_stopspammingconsole)
            {
                Console.WriteLine($"Font file not found at: {fontPath}");
            }
        }
    }
    #endregion

}
