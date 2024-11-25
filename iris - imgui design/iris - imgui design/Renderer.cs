using ClickableTransparentOverlay;
using ConfigSys;
using ImGuiNET;
using System.Numerics;
using System.Runtime.InteropServices;


namespace IrisRenderer
{
    public class Renderer : Overlay
    {
        #region Settings
        #region Unlisted
        public bool showMenu = true;
        public bool fovChanger = false;
        public float fov = 65;
        private float Dfov = 65;
        private float perfectfov = 120;
        public bool noRecoil = false;
        public bool autoBhop = false;
        public bool showSpeed = false;
        #endregion

        #region Crosshair
        public bool crosshair = false;
        public float size = 20;
        public float thickness = 2;
        public float outlineThickness = 2;

        public float opacity = 1;
        public float circleOpacity = 1;
        // Default
        public float Dsize = 20;
        public float Dthickness = 2;
        public float DoutlineThickness = 2;
        public float Dopacity = 1;
        public float DcircleOpacity = 1;
        public float DcircleOutlineThickness = 2;
        public Vector4 crosshairColor = new Vector4(0, 1, 0, 1);
        public Vector4 circleColor = new Vector4(1, 1, 1, 1);
        public Vector4 outlineColor = new Vector4(0, 0, 0, 1);
        #endregion

        #region ESP
        public bool enableAllESP
        {
            get
            {
                return headESP && boneESP && boxESP && healthBarESP &&
                       trajectoryESP && nameESP && weaponESP && distanceESP &&
                       shipESP && gruntESP && reaperESP && stalkerESP &&
                       suicideSpectreESP && tickESP && mrvnESP && pilotESP && titanESP;
            }
            set
            {
                headESP = value;
                boneESP = value;
                boxESP = value;
                healthBarESP = value;
                trajectoryESP = value;
                nameESP = value;
                weaponESP = value;
                distanceESP = value;
                shipESP = value;
                gruntESP = value;
                reaperESP = value;
                stalkerESP = value;
                suicideSpectreESP = value;
                tickESP = value;
                mrvnESP = value;
                pilotESP = value;
                titanESP = value;
            }
        }

        public bool esp = false;
        public bool headESP = false;
        public bool boneESP = false;
        public bool boxESP = false;
        public bool cornerESP = false;
        public bool healthBarESP = false;
        public bool healthTextESP = false;
        public bool trajectoryESP = false;
        public bool nameESP = false;
        public bool weaponESP = false;
        public bool distanceESP = false;

        public bool shipESP = false;
        public bool gruntESP = false;
        public bool reaperESP = false;
        public bool stalkerESP = false;
        public bool suicideSpectreESP = false;
        public bool tickESP = false;
        public bool mrvnESP = false;

        public bool pilotESP = false;
        public bool titanESP = false;
        public bool aiESP = false;
        #endregion

        #region Aimbot
        public bool aimbot = false;
        public bool enableAllAimbot = false;
        public bool pilotAimbot = false;
        public bool titanAimbot = false;
        public bool aiAimbot = false;
        public bool predictionAimbot = false;
        public float aimbotSmoothing = 0;
        public float DaimbotSmoothing = 0;
        public float aimbotFovCircle = 10;
        public float DaimbotFovCircle = 10;
        public bool showAimbotFovCircle = false;
        public bool triggerbot = false;
        public int triggerbotDelay = 200;
        public bool aimAtHead = false;
        public bool aimAtNearest = false;
        public bool aimAtBody = false;
        public bool aimAtTitanCrit = false;
        public bool aimAtTitanNearest = false;
        public bool aimAtTitanBody = false;
        #endregion

        #region Drawing
        ImDrawListPtr _drawlist;
        bool InputFloatSm = false;
        int yOffs = 20;
        int xOffs = 0;
        int WxOffs = 0;
        int WyOffs = 0;

        public uint GetColorWithOpacity(Vector4 color, float opacity)
        {
            return ((uint)(opacity * 255) << 24) |  // Alpha (opacity)
                   ((uint)(color.Z * 255) << 16) |  // Blue
                   ((uint)(color.Y * 255) << 8) |  // Green
                   ((uint)(color.X * 255));         // Red
        }
        public void RenderOverlay()
        {
            _drawlist = ImGui.GetForegroundDrawList();
            var viewport = ImGui.GetMainViewport();
            var centerX = viewport.Size.X / 2 + viewport.Pos.X;
            var centerY = viewport.Size.Y / 2 + viewport.Pos.Y;

            // Adjust color with opacity
            uint drawCrosshairColor = GetColorWithOpacity(crosshairColor, opacity);
            uint drawCircleColor = GetColorWithOpacity(circleColor, circleOpacity);
            uint drawOutlineColor = GetColorWithOpacity(outlineColor, opacity);

            // Conditionally render crosshair elements if 'crosshair' is enabled
            if (crosshair)
            {
                // Draw outline for vertical line
                _drawlist.AddLine(
                    new Vector2(centerX, centerY - size / 2 - outlineThickness / 2),
                    new Vector2(centerX, centerY + size / 2 + outlineThickness / 2),
                    drawOutlineColor, outlineThickness);

                // Draw outline for horizontal line
                _drawlist.AddLine(
                    new Vector2(centerX - size / 2 - outlineThickness / 2, centerY),
                    new Vector2(centerX + size / 2 + outlineThickness / 2, centerY),
                    drawOutlineColor, outlineThickness);

                // Draw the vertical line
                _drawlist.AddLine(
                    new Vector2(centerX, centerY - size / 2),
                    new Vector2(centerX, centerY + size / 2),
                    drawCrosshairColor, thickness);

                // Draw the horizontal line
                _drawlist.AddLine(
                    new Vector2(centerX - size / 2, centerY),
                    new Vector2(centerX + size / 2, centerY),
                    drawCrosshairColor, thickness);
            }

            // Conditionally draw FOV circle if enabled
            if (showAimbotFovCircle)
            {
                _drawlist.AddCircle(
                   new Vector2(centerX, centerY), aimbotFovCircle, drawCircleColor);
            }
        }
        #endregion
        #endregion
        #region Defining Classes
        private ImGuiStyleConfig styleConfig = new ImGuiStyleConfig();
        private Config configSys = new Config();
        #endregion

        #region Constructor
        public Renderer() : base(1920, 1080) // 1280 x 720, 1920 x 1080, 2560 x 1440
        {
        }
        #endregion

        public void Write2MemWarning()
        {
            ImGui.SameLine();
            ImGui.TextColored(new Vector4(1.0f, 0.0f, 0.0f, 1.0f), "(WARNING)");
            if (ImGui.IsItemHovered()) ImGui.SetTooltip("READ/WRITES TO MEMORY!!");
        }

        #region Main ImGui Loop
        protected override void Render()
        {
            if ((GetAsyncKeyState(VK_INSERT) & 0x8000) != 0)
            {
                showMenu = !showMenu;
                Thread.Sleep(150);
            }

            if (!showMenu) return;

            ImGui.Begin("Iris - Titanfall 2", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoTitleBar);
            styleConfig.ApplyStyle();
            //styleConfig.LoadFont(); //sigh still cant get the font to load...
            ImGui.TextColored(new Vector4(1.0f, 0.8f, 0.0f, 1.0f), "Iris - WIP");
            ImGui.SameLine();
            ImGui.TextColored(new Vector4(0.24f, 0.24f, 0.24f, 1.0f), "v1.0.0.a");
            if (ImGui.BeginTabBar("MainTabs"))
            {
                #region Main Tab
                if (ImGui.BeginTabItem("Main"))
                {
                    ImGui.Checkbox("Enable Aimbot", ref aimbot);
                    ImGui.Checkbox("Enable ESP", ref esp);
                    ImGui.Checkbox("No Recoil", ref noRecoil);
                    Write2MemWarning();
                    ImGui.Checkbox("Auto Bhop", ref autoBhop);
                    Write2MemWarning();
                    ImGui.Checkbox("FOV Changer", ref fovChanger);
                    Write2MemWarning();
                    ImGui.Checkbox("Crosshair", ref crosshair);
                    ImGui.Checkbox("Show Speed", ref showSpeed);
                    Write2MemWarning();
                    ImGui.EndTabItem();
                }
                #endregion
                #region Options Tab
                if (esp || aimbot || fovChanger || crosshair)
                {
                    if (ImGui.BeginTabItem("Options"))
                    {
                        #region Aimbot
                        if (aimbot)
                        {
                            if (ImGui.CollapsingHeader("Aimbot Options"))
                            {
                                ImGui.Indent();
                                ImGui.SliderFloat("Smoothing", ref aimbotSmoothing, 0, 100);

                                ImGui.SameLine();
                                if (ImGui.Button("Reset##Smoothing")) aimbotSmoothing = DaimbotSmoothing;
                                ImGui.Checkbox("Pilot Aimbot", ref pilotAimbot);
                                if (pilotAimbot)
                                {
                                    ImGui.Indent();
                                    if (ImGui.CollapsingHeader("Pilot bones to aim at"))
                                    {
                                        ImGui.Checkbox("Aim at Head", ref aimAtHead);
                                        ImGui.Checkbox("Aim at Body", ref aimAtBody);
                                        ImGui.Checkbox("Aim at Nearest Bone", ref aimAtNearest);
                                    }
                                    ImGui.Unindent();
                                }
                                ImGui.Checkbox("Titan Aimbot", ref titanAimbot);
                                if (titanAimbot)
                                {
                                    ImGui.Indent();
                                    if (ImGui.CollapsingHeader("Titan bones to aim at"))
                                    {
                                        ImGui.Checkbox("Aim at Critical spots", ref aimAtTitanCrit);
                                        ImGui.Checkbox("Aim at Body", ref aimAtTitanBody);
                                        ImGui.Checkbox("Aim at Nearest Bone", ref aimAtTitanNearest);
                                    }
                                    ImGui.Unindent();
                                }
                                ImGui.Checkbox("AI Aimbot", ref aiAimbot);
                                ImGui.SameLine();
                                ImGui.TextColored(new Vector4(0.24f, 0.24f, 0.24f, 1.0f), "(?)");
                                if (ImGui.IsItemHovered()) ImGui.SetTooltip("Target's Grunts, Reapers, Ticks, Spectres, and Stalkers.");
                                ImGui.Checkbox("Prediction Aimbot", ref predictionAimbot);
                                ImGui.Checkbox("Triggerbot", ref triggerbot);
                                if (triggerbot)
                                {
                                    ImGui.SliderInt("Triggerbot Delay (in ms)", ref triggerbotDelay, 0, 1000);
                                }
                                ImGui.Checkbox("Show Aimbot Fov Circle", ref showAimbotFovCircle);
                                if (showAimbotFovCircle)
                                {
                                    if (ImGui.CollapsingHeader("FOV Circle Customization"))
                                    {
                                        ImGui.Indent();
                                        ImGui.SliderFloat("FOV Circle", ref aimbotFovCircle, 10, 1000);
                                        ImGui.SameLine();
                                        if (ImGui.Button("Reset##FOVCircle")) aimbotFovCircle = DaimbotFovCircle;
                                        ImGui.SliderFloat("FOV Circle Opacity", ref circleOpacity, 0f, 1f, "%.2f");
                                        ImGui.SameLine();
                                        if (ImGui.Button("Reset##FOVCircleOpacity")) circleOpacity = DcircleOpacity;
                                        ImGui.ColorEdit4("##circlecolor", ref circleColor);
                                        ImGui.Unindent();
                                    }
                                }
                                ImGui.Unindent();
                            }
                        }
                        #endregion
                        #region ESP
                        if (esp)
                        {
                            if (ImGui.CollapsingHeader("ESP Options"))
                            {
                                ImGui.Indent();
                                ImGui.Checkbox("Head", ref headESP);
                                ImGui.Checkbox("Bone", ref boneESP);
                                ImGui.Checkbox("Health Bar", ref healthBarESP);
                                ImGui.Checkbox("Box", ref boxESP);
                                ImGui.Checkbox("Trajectory", ref trajectoryESP);
                                ImGui.Checkbox("Name", ref nameESP);
                                ImGui.Checkbox("Weapon", ref weaponESP);
                                ImGui.Checkbox("Distance", ref distanceESP);
                                if (ImGui.CollapsingHeader("AI ESP"))
                                {
                                    ImGui.Indent();
                                    ImGui.Checkbox("Grunt", ref gruntESP);
                                    ImGui.Checkbox("Reaper", ref reaperESP);
                                    ImGui.Checkbox("Ship", ref shipESP);
                                    ImGui.Checkbox("Stalker", ref stalkerESP);
                                    ImGui.Checkbox("Spectre", ref suicideSpectreESP);
                                    ImGui.Checkbox("Tick", ref tickESP);
                                    ImGui.Checkbox("Mrvn", ref mrvnESP);
                                    ImGui.Unindent();
                                }
                                ImGui.Unindent();
                            }
                        }
                        #endregion
                        #region Crosshair
                        if (crosshair)
                        {
                            if (ImGui.CollapsingHeader("Crosshair Customization"))
                            {
                                ImGui.Indent();
                                ImGui.SliderFloat("Size", ref size, 1, 100.0f, "%.1f");
                                ImGui.SameLine();
                                if (ImGui.Button("Reset##Size")) size = Dsize;
                                ImGui.SliderFloat("Thickness", ref thickness, 0.1f, 20.0f, "%.1f");
                                ImGui.SameLine();
                                if (ImGui.Button("Reset##Thickness")) thickness = Dthickness;
                                ImGui.SliderFloat("OutlineThickness", ref outlineThickness, 0.1f, 20.0f, "%.1f");
                                ImGui.SameLine();
                                if (ImGui.Button("Reset##OutlineThickness")) outlineThickness = DoutlineThickness;
                                ImGui.SliderFloat("Crosshair Opacity", ref opacity, 0f, 1f, "%.2f");
                                ImGui.SameLine();
                                if (ImGui.Button("Reset##CrosshairOpacity")) opacity = Dopacity;
                                ImGui.ColorEdit4("##crosshaircolor", ref crosshairColor);
                                ImGui.SameLine();
                                ImGui.TextColored(new Vector4(0.24f, 0.24f, 0.24f, 1.0f), "(?)");
                                if (ImGui.IsItemHovered()) ImGui.SetTooltip("Main Crosshair Color");
                                ImGui.ColorEdit4("##outlinecolor", ref outlineColor);
                                ImGui.SameLine();
                                ImGui.TextColored(new Vector4(0.24f, 0.24f, 0.24f, 1.0f), "(?)");
                                if (ImGui.IsItemHovered()) ImGui.SetTooltip("Outline Color");
                                ImGui.Unindent();
                            }
                        }
                        #endregion
                        #region FOV
                        if (fovChanger)
                        {
                            if (ImGui.CollapsingHeader("FOV Options"))
                            {
                                ImGui.Indent();
                                ImGui.SliderFloat("Field of view", ref fov, 1, 180);
                                ImGui.SameLine();
                                if (ImGui.Button("Reset##Fieldofview")) fov = Dfov;
                                if (ImGui.Button("120 Fov##Fieldofview")) fov = perfectfov;
                                ImGui.Unindent();
                            }
                        }
                        #endregion
                        ImGui.EndTabItem();
                    }
                }
                #endregion
                /*
                #region Config System
                if (ImGui.BeginTabItem("Configs"))
                {
                    ImGui.Spacing();
                    ImGui.Text("Available Configs:");
                    foreach (var configName in configList)
                    {
                        if (ImGui.Button(configName))
                        {
                            config = ConfigManager.LoadConfig(configName);
                            currentConfigName = configName;
                            LoadConfigValues(config);
                        }
                    }

                    ImGui.InputText("New Config Name", ref newConfigName, 100);
                    ImGui.Spacing();
                    if (ImGui.Button("Save Current Config"))
                    {
                        if (!string.IsNullOrEmpty(newConfigName))
                        {
                            SetConfigValues(config);
                            ConfigManager.SaveConfig(newConfigName, config);
                            configList = ConfigManager.GetConfigList();
                        }
                    }

                    ImGui.EndTabItem();
                }
                #endregion
                */
                ImGui.EndTabBar();

            }
            ImGui.End();
            if (crosshair || showAimbotFovCircle) RenderOverlay();

        }
        #endregion

        #region Using User32.dll to get INSERT key
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);
        private const int VK_INSERT = 0x2D;
        #endregion
    }
    #region compiler will whine if i dont have this. (shitty vs)
    class Program
    {
        static void Main(string[] args)
        {
            Renderer renderer = new Renderer();
            renderer.Run();
        }
    }
    #endregion
}