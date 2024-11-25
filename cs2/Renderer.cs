using ClickableTransparentOverlay;
using ImGuiNET;
using System.Collections.Concurrent;
using System.Numerics;
namespace CS2
{
    internal class Renderer : Overlay
    {
        private ImGuiStyleConfig styleConfig = new ImGuiStyleConfig();
        public bool isRunning = true;

        #region settings variables
        public bool FovEnabled = false;
        public bool AutoGetFovWhileDisabled = false;
        public int fov = 60;
        public bool LessCpuUsageButLag = false;
        bool showFov = false;
        bool showplus = false;
        public bool FlashCheckAim = false, FlashCheckTrigger = false;

        public ConcurrentQueue<string> listLog = new ConcurrentQueue<string>();
        public int bob = 5;
        #endregion
        #region skills variables
        public bool Radar = false;
        public bool NoFlash = false;
        public bool Triggerbot = false;

        public int TBHotKey = 0x10;
        public int AbHotKey = 0x02;
        public int AJHotKey = 0x09;
        public int BHHotKey = 0x20;

        int TBIndex = 6;
        int ABIndex = 1;
        int AJIndex = 6;
        int BHIndex = 10;

        string[] Options =
{
            "Left Click",
            "Right Click",
            "Middle Click",
            "X1/Button 4",
            "X2/Button 5",
            "Tab",
            "Shift",
            "Ctrl",
            "Caps",
            "Alt",
            "Space",
            "Left Shift",
            "Right Shift",
                        "0", "1", "2", "3", "4", "5", "6", "7",
            "8", "9", "A", "B", "C", "D", "E ", "F",
            "G", "H", "I", "J", "K", "L", "M", "N",
            "O", "P", "Q", "R", "S", "T", "U", "V ",
            "W", "X", "Y", "Z"
        };
        public int[] Keys =
        {
            0x01,
            0x02,
            0x04,
            0x05,
            0x06,
            0x09,
            0x10,
            0x11,
            0x14,
            0x12,
            0x20,
            0xA0 ,
              0xA1  ,
                      0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39,
            0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A,
            0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54,
            0x55, 0x56, 0x57, 0x58, 0x59, 0x5A
        };
        #endregion
        #region Status variables        
        public bool enablemoneyChecking = false;
        public bool bombPlanted = false;
        public int timeLeft = -1;
        public int speed = 0;
        int maxSpeed = 0;
        public bool EnableStatus = false;
        #endregion
        #region aimbot variables
        public bool AimbotEnabled = false;
        public bool shootOnjumpShot = false;
        public bool AimOnTeam = false;
        public float AimbotFOV = 30;
        public bool aimOnlyatSpotted = true;
        public float smoothness = 0.001f;
        public bool visibleColorChangeFOV;
        public bool EntityVisible = false;
        public Vector4 invisibleColor = new Vector4(0, 0, 0, 1);
        #endregion

        public List<Entity> entities = new List<Entity>();
        public List<Entity> nentities = new List<Entity>();

        public Vector2 screenSize = new Vector2(1920, 1080);
        public Vector4 circleColor = new Vector4(1, 1, 1, 1);

        #region ESP
        bool onlyRenderBoneHead = false;
        public bool AutoBHOP = false;
        public bool WeaponESP = false;
        public Vector4 DistanceEnemyColor = new Vector4(1, 0, 0, 1);
        public Vector4 DistanceTeamColor = new Vector4(0, 1, 0, 1);
        public bool EnableDistanceCheck = false, teanD = false;
        int DyOffs = 20;
        int DxOffs = 26;
        //box
        public ConcurrentQueue<Entity> espentities = new ConcurrentQueue<Entity>();
        private Entity localPlayer = new Entity();
        private readonly object entityLock = new object();

        public bool BoxEsp = false;
        public bool EnableTracklines = false;
        public bool HealthBar = false;
        public bool BoneEsp = false;
        public bool TeamEsp = false;
        public bool NameEsp = false;
        public bool DisplayTeamName = false, DisplayTeamWeapon = false, ShowTeamBox = false, ShowTeamTrackline = false, ShowTeamHealth = false, showTeamBone = false;
        public Vector4 BoneColors = new Vector4(1, 1, 1, 1);
        public Vector4 TeamBoneColors = new Vector4(1, 1, 1, 1);
        public Vector4 HeadEnemy = new Vector4(1, 1, 1, 1);
        public Vector4 HeadTeam = new Vector4(1, 1, 1, 1);
        public Vector4 UnVisibleHeadColor = new Vector4(0, 0, 0, 1);
        public bool VisibiltyHeadCheck = false, addFace = false;
        public float FaceSize = 16.944f;
        public bool displayTeamHead = false, HeadEsps = false;
        Vector4 boxColorteam = new Vector4(0, 1, 0, 1), boxColorEnemy = new Vector4(1, 0, 0, 1);
        Vector4 tracklinesColorTeam = new Vector4(0, 1, 0, 1), tracklinesColorEnemy = new Vector4(1, 0, 0, 1);

        Vector4 healthColorTeam = new Vector4(0, 1, 0, 1), healthColorEnemy = new Vector4(0, 1, 0, 1);
        Vector4 healthColorT50 = new Vector4(1, 0, 0, 1), healthColorE50 = new Vector4(1, 0, 0, 1);
        Vector4 WEECOLOR = new Vector4(1, 0, 0, 1), WTTCOLOR = new Vector4(0, 1, 0, 1);
        bool xx = false;
        bool NotTSpotted = false, NotBSpotted = false, NotNSpotted = false, NotBoneSpotted = false;
        Vector4 NTS = new Vector4(0, 0, 0, 1);
        Vector4 NBS = new Vector4(0, 0, 0, 1);
        Vector4 NNS = new Vector4(0, 0, 0, 1);
        Vector4 NBoneS = new Vector4(0, 0, 0, 1);

        float boneThickness = 1;
        float headThickness = 4.667f;
        float healthThickness = 5, healthroundness = 0;
        float tracklineThickness = 1;
        float boxThickness = 1, boxRoundness = 0;
        public Vector4 NameEspEnemy = new Vector4(1, 0, 0, 1);
        public Vector4 NameEspTeam = new Vector4(0, 1, 0, 1);
        public int delay = 1;
        #endregion
        #region crosshair
        private float size = 20.3f;
        private float thickness = 0.1f;
        private Vector4 color = new Vector4(1f, 1f, 1f, 1f);
        private Vector4 outlineColor = new Vector4(0f, 0f, 0f, 1f);
        private float outlineThickness = 2.5f;
        private float centerDotSize = 1f;
        private float opacity = 1f;
        #endregion
        #region color variables
        Vector4 redColor = new Vector4(1, 0, 0, 1);
        Vector4 greenColor = new Vector4(0, 1, 0, 1);
        Vector4 yellowColor = new Vector4(1, 1, 0, 1);
        Vector4 white = new Vector4(1, 1, 1, 1);
        #endregion

        ImDrawListPtr _drawlist;
        bool InputFloatSm = false;
        int yOffs = 20;
        int xOffs = 0;
        int WxOffs = 0;
        int WyOffs = 0;
        public List<string> Currentspectators = new List<string>();

        int TopBoxHeight = 0;
        int TopBoxWidth = 0;
        int BottomBoxHeight = 0;
        int BottomBoxWidth = 0;

        int tracklinePosX = 0;
        int tracklinePosY = 0;

        int startXpos = 0;
        int startYpos = 0;
        public bool spectatorsEnabled = false;
        static Config config = new Config();
        static string currentConfigName = "default";
        static string newConfigName = "";
        static List<string> configList = new List<string>();
        public void SetConfigValues(Config config)
        {
            if (newConfigName == string.Empty)
                config.ConfigName = currentConfigName + "1";
            else
                config.ConfigName = newConfigName;
            config.spectatorsEnabled = spectatorsEnabled;
            config.AimbotEnabled = AimbotEnabled;
            config.AimOnTeam = AimOnTeam;
            config.AimOnlyAtSpotted = aimOnlyatSpotted;
            config.FlashCheckAim = FlashCheckAim;
            config.InputFloatSm = InputFloatSm;
            config.Smoothness = smoothness;
            config.ABIndex = ABIndex;
            config.ShowFov = showFov;
            config.AimbotFOV = AimbotFOV;
            config.VisibleColorChangeFOV = visibleColorChangeFOV;
            config.InvisibleColor = invisibleColor;
            config.CircleColor = circleColor;
            config.Triggerbot = Triggerbot;
            config.FlashCheckTrigger = FlashCheckTrigger;
            config.TBIndex = TBIndex;
            config.AutoBHOP = AutoBHOP;
            config.Bob = bob;
            config.BHIndex = BHIndex;
            config.ShootOnJumpShot = shootOnjumpShot;
            config.AJIndex = AJIndex;
            config.BoxEsp = BoxEsp;
            config.ShowTeamBox = ShowTeamBox;
            config.NotBSpotted = NotBSpotted;
            config.NBS = NBS;
            config.BoxThickness = boxThickness;
            config.BoxRoundness = boxRoundness;
            config.TopBoxHeight = TopBoxHeight;
            config.TopBoxWidth = TopBoxWidth;
            config.BottomBoxHeight = BottomBoxHeight;
            config.BottomBoxWidth = BottomBoxWidth;
            config.BoxColorEnemy = boxColorEnemy;
            config.BoxColorTeam = boxColorteam;
            config.EnableTracklines = EnableTracklines;
            config.ShowTeamTrackline = ShowTeamTrackline;
            config.NotTSpotted = NotTSpotted;
            config.NTS = NTS;
            config.TracklinePosX = tracklinePosX;
            config.TracklinePosY = tracklinePosY;
            config.StartXPos = startXpos;
            config.StartYPos = startXpos;
            config.TracklineThickness = tracklineThickness;
            config.TracklinesColorEnemy = tracklinesColorEnemy;
            config.TracklinesColorTeam = tracklinesColorTeam;
            config.HealthBar = HealthBar;
            config.ShowTeamHealth = ShowTeamHealth;
            config.HealthThickness = healthThickness;
            config.HealthRoundness = healthroundness;
            config.HealthColorEnemy = healthColorEnemy;
            config.HealthColorE50 = healthColorE50;
            config.HealthColorTeam = healthColorTeam;
            config.HealthColorT50 = healthColorT50;
            config.BoneEsp = BoneEsp;
            config.ShowTeamBone = showTeamBone;
            config.NotBoneSpotted = NotBoneSpotted;
            config.OnlyRenderBoneHead = onlyRenderBoneHead;
            config.NBoneS = NBoneS;
            config.BoneThickness = boneThickness;
            config.BoneColors = BoneColors;
            config.TeamBoneColors = TeamBoneColors;
            config.HeadEsps = HeadEsps;
            config.DisplayTeamHead = displayTeamHead;
            config.VisibilityHeadCheck = VisibiltyHeadCheck;
            config.AddFace = addFace;
            config.FaceSize = FaceSize;
            config.UnVisibleHeadColor = UnVisibleHeadColor;
            config.HeadThickness = headThickness;
            config.HeadTeam = HeadTeam;
            config.HeadEnemy = HeadEnemy;
            config.NameEsp = NameEsp;
            config.DisplayTeamName = DisplayTeamName;
            config.NotNSpotted = NotNSpotted;
            config.NNS = NNS;
            config.YOffs = yOffs;
            config.XOffs = xOffs;
            config.NameEspEnemy = NameEspEnemy;
            config.NameEspTeam = NameEspTeam;
            config.WeaponESP = WeaponESP;
            config.DisplayTeamWeapon = DisplayTeamWeapon;
            config.WyOffs = WyOffs;
            config.WxOffs = WxOffs;
            config.WEECOLOR = WEECOLOR;
            config.WTTCOLOR = WTTCOLOR;
            config.EnableDistanceCheck = EnableDistanceCheck;
            config.TeanD = teanD;
            config.DyOffs = DyOffs;
            config.DxOffs = DxOffs;
            config.DistanceEnemyColor = DistanceEnemyColor;
            config.DistanceTeamColor = DistanceTeamColor;
            config.Radar = Radar;
            config.NoFlash = NoFlash;
            config.FovEnabled = FovEnabled;
            config.Fov = fov;
            config.ShowPlus = showplus;
            config.Size = size;
            config.Thickness = thickness;
            config.Color = color;
            config.OutlineThickness = outlineThickness;
            config.OutlineColor = outlineColor;
            config.CenterDotSize = centerDotSize;
            config.Opacity = opacity;
            config.EnableStatus = EnableStatus;
            config.EnableMoneyChecking = enablemoneyChecking;
            config.LessCpuUsageButLag = LessCpuUsageButLag;
            config.XX = xx;
            config.Delay = delay;
        }
        public void LoadConfigValues(Config config)
        {
            currentConfigName = config.ConfigName;
            spectatorsEnabled = config.spectatorsEnabled;
            AimbotEnabled = config.AimbotEnabled;
            AimOnTeam = config.AimOnTeam;
            aimOnlyatSpotted = config.AimOnlyAtSpotted;
            FlashCheckAim = config.FlashCheckAim;
            InputFloatSm = config.InputFloatSm;
            smoothness = config.Smoothness;
            ABIndex = config.ABIndex;
            showFov = config.ShowFov;
            AimbotFOV = config.AimbotFOV;
            visibleColorChangeFOV = config.VisibleColorChangeFOV;
            invisibleColor = config.InvisibleColor;
            circleColor = config.CircleColor;
            Triggerbot = config.Triggerbot;
            FlashCheckTrigger = config.FlashCheckTrigger;
            TBIndex = config.TBIndex;
            AutoBHOP = config.AutoBHOP;
            bob = config.Bob;
            BHIndex = config.BHIndex;
            shootOnjumpShot = config.ShootOnJumpShot;
            AJIndex = config.AJIndex;
            BoxEsp = config.BoxEsp;
            ShowTeamBox = config.ShowTeamBox;
            NotBSpotted = config.NotBSpotted;
            NBS = config.NBS;
            boxThickness = config.BoxThickness;
            boxRoundness = config.BoxRoundness;
            TopBoxHeight = config.TopBoxHeight;
            TopBoxWidth = config.TopBoxWidth;
            BottomBoxHeight = config.BottomBoxHeight;
            BottomBoxWidth = config.BottomBoxWidth;
            boxColorEnemy = config.BoxColorEnemy;
            boxColorteam = config.BoxColorTeam;
            EnableTracklines = config.EnableTracklines;
            ShowTeamTrackline = config.ShowTeamTrackline;
            NotTSpotted = config.NotTSpotted;
            NTS = config.NTS;
            tracklinePosX = config.TracklinePosX;
            tracklinePosY = config.TracklinePosY;
            startXpos = config.StartXPos;
            startYpos = config.StartYPos;
            tracklineThickness = config.TracklineThickness;
            tracklinesColorEnemy = config.TracklinesColorEnemy;
            tracklinesColorTeam = config.TracklinesColorTeam;
            HealthBar = config.HealthBar;
            ShowTeamHealth = config.ShowTeamHealth;
            healthThickness = config.HealthThickness;
            healthroundness = config.HealthRoundness;
            healthColorEnemy = config.HealthColorEnemy;
            healthColorE50 = config.HealthColorE50;
            healthColorTeam = config.HealthColorTeam;
            healthColorT50 = config.HealthColorT50;
            BoneEsp = config.BoneEsp;
            showTeamBone = config.ShowTeamBone;
            NotBoneSpotted = config.NotBoneSpotted;
            onlyRenderBoneHead = config.OnlyRenderBoneHead;
            NBoneS = config.NBoneS;
            boneThickness = config.BoneThickness;
            BoneColors = config.BoneColors;
            TeamBoneColors = config.TeamBoneColors;
            HeadEsps = config.HeadEsps;
            displayTeamHead = config.DisplayTeamHead;
            VisibiltyHeadCheck = config.VisibilityHeadCheck;
            addFace = config.AddFace;
            FaceSize = config.FaceSize;
            UnVisibleHeadColor = config.UnVisibleHeadColor;
            headThickness = config.HeadThickness;
            HeadTeam = config.HeadTeam;
            HeadEnemy = config.HeadEnemy;
            NameEsp = config.NameEsp;
            DisplayTeamName = config.DisplayTeamName;
            NotNSpotted = config.NotNSpotted;
            NNS = config.NNS;
            yOffs = config.YOffs;
            xOffs = config.XOffs;
            NameEspEnemy = config.NameEspEnemy;
            NameEspTeam = config.NameEspTeam;
            WeaponESP = config.WeaponESP;
            DisplayTeamWeapon = config.DisplayTeamWeapon;
            WyOffs = config.WyOffs;
            WxOffs = config.WxOffs;
            WEECOLOR = config.WEECOLOR;
            WTTCOLOR = config.WTTCOLOR;
            EnableDistanceCheck = config.EnableDistanceCheck;
            teanD = config.TeanD;
            DyOffs = config.DyOffs;
            DxOffs = config.DxOffs;
            DistanceEnemyColor = config.DistanceEnemyColor;
            DistanceTeamColor = config.DistanceTeamColor;
            Radar = config.Radar;
            NoFlash = config.NoFlash;
            FovEnabled = config.FovEnabled;
            fov = config.Fov;
            showplus = config.ShowPlus;
            size = config.Size;
            thickness = config.Thickness;
            color = config.Color;
            outlineThickness = config.OutlineThickness;
            outlineColor = config.OutlineColor;
            centerDotSize = config.CenterDotSize;
            opacity = config.Opacity;
            EnableStatus = config.EnableStatus;
            enablemoneyChecking = config.EnableMoneyChecking;
            LessCpuUsageButLag = config.LessCpuUsageButLag;
            xx = config.XX;
            delay = config.Delay;
        }

        protected override void Render()
        {
            config = ConfigManager.LoadConfig(currentConfigName);
            configList = ConfigManager.GetConfigList();
            if (speed > maxSpeed)
                maxSpeed = speed;

            ImGui.Begin("Iris - CS2", ref isRunning);
            ImGui.TextColored(new Vector4(1.0f, 0.8f, 0.0f, 1.0f), "Iris CS2 - WIP");
            ImGui.SameLine();
            ImGui.TextColored(new Vector4(0.24f, 0.24f, 0.24f, 1.0f), "v1.1.2.a");
            styleConfig.ApplyStyle();

            if (ImGui.BeginTabBar("##MainTabs"))
            {

                if (ImGui.BeginTabItem("Aimbot"))
                {
                    ImGui.Spacing();
                    ImGui.Checkbox("Enable Aimbot", ref AimbotEnabled);
                    ImGui.Spacing();
                    ImGui.Text("Options");
                    ImGui.Checkbox("Aim At Teamates", ref AimOnTeam);
                    ImGui.Checkbox("Visible Only", ref aimOnlyatSpotted);
                    ImGui.Checkbox("Aim Flash Check", ref FlashCheckAim);

                    if (InputFloatSm)
                    {
                        ImGui.Checkbox("Input", ref InputFloatSm);
                        ImGui.InputFloat("Speed", ref smoothness);
                    }
                    else
                    {
                        ImGui.Checkbox("Input", ref InputFloatSm);
                        ImGui.SliderFloat("Speed", ref smoothness, 0.01f, 999);
                    }
                    ImGui.Spacing();

                    ImGui.Spacing();
                    ImGui.Text("Input");
                    if (ImGui.BeginCombo("Aim Key", Options[ABIndex]))
                    {
                        for (int i = 0; i < Options.Length; i++)
                        {
                            bool isSelected = (ABIndex == i);
                            if (ImGui.Selectable(Options[i], isSelected))
                            {
                                ABIndex = i;
                                AbHotKey = Keys[i];
                            }
                            if (isSelected)
                            {
                                ImGui.SetItemDefaultFocus();
                            }
                        }
                        ImGui.EndCombo();
                    }
                    ImGui.Spacing();

                    ImGui.Spacing();
                    ImGui.Text("FOV Settings");

                    ImGui.Checkbox("Draw FOV", ref showFov);
                    ImGui.SliderFloat("Fov Size", ref AimbotFOV, 10, 999);
                    ImGui.Checkbox("Visibility Check", ref visibleColorChangeFOV);
                    if (visibleColorChangeFOV)
                    {
                        ImGui.Text("Invisible");
                        ImGui.ColorEdit4("##Invisible Color", ref invisibleColor);
                        ImGui.Text("Visible");
                    }
                    else
                    {
                        ImGui.Text("Color");
                    }

                    ImGui.ColorEdit4("##Circle Color", ref circleColor);
                    ImGui.EndTabItem();

                }

                if (ImGui.BeginTabItem("Skills"))
                {
                    ImGui.Spacing();
                    ImGui.Checkbox("Triggerbot", ref Triggerbot);
                    if (Triggerbot)
                    {
                        ImGui.Checkbox("Trigger Flash Check", ref FlashCheckTrigger);
                    }

                    ImGui.Text("Input");
                    if (ImGui.BeginCombo("##a", Options[TBIndex]))
                    {
                        for (int i = 0; i < Options.Length; i++)
                        {
                            bool isSelected = (TBIndex == i);
                            if (ImGui.Selectable(Options[i], isSelected))
                            {
                                TBIndex = i;
                                TBHotKey = Keys[i];
                            }
                            if (isSelected)
                            {
                                ImGui.SetItemDefaultFocus();
                            }
                        }
                        ImGui.EndCombo();
                    }
                    ImGui.Spacing();
                    ImGui.Spacing();
                    ImGui.Checkbox("B-Hopping", ref AutoBHOP);
                    if (AutoBHOP)
                    {
                        ImGui.SliderInt("Delay", ref bob, 0, 100);
                    }
                    ImGui.Text("Input");
                    if (ImGui.BeginCombo("##b", Options[BHIndex]))
                    {
                        for (int i = 0; i < Options.Length; i++)
                        {
                            bool isSelected = (BHIndex == i);
                            if (ImGui.Selectable(Options[i], isSelected))
                            {
                                BHIndex = i;
                                BHHotKey = Keys[i];
                            }
                            if (isSelected)
                            {
                                ImGui.SetItemDefaultFocus();
                            }
                        }
                        ImGui.EndCombo();
                    }
                    ImGui.Spacing();
                    ImGui.Spacing();
                    ImGui.Checkbox("Auto Jump Shot", ref shootOnjumpShot);
                    ImGui.Text("Input");
                    if (ImGui.BeginCombo("##c", Options[AJIndex]))
                    {
                        for (int i = 0; i < Options.Length; i++)
                        {
                            bool isSelected = (AJIndex == i);
                            if (ImGui.Selectable(Options[i], isSelected))
                            {
                                AJIndex = i;
                                AJHotKey = Keys[i];
                            }
                            if (isSelected)
                            {
                                ImGui.SetItemDefaultFocus();
                            }
                        }
                        ImGui.EndCombo();
                    }
                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem("Esp"))
                {
                    ImGui.Spacing();
                    ImGui.Checkbox("Box ESP Enabled", ref BoxEsp);
                    if (BoxEsp && ImGui.TreeNode("Box Esp Settings"))
                    {
                        ImGui.Checkbox("Team Box Enabled", ref ShowTeamBox);
                        ImGui.Checkbox("Box Visibility Check", ref NotBSpotted);
                        if (NotBSpotted)
                        {
                            ImGui.Text("Invisible Color");
                            ImGui.ColorEdit4("##Boxes", ref NBS);
                        }

                        ImGui.Text("Enemy");
                        ImGui.ColorEdit4("##BoxE", ref boxColorEnemy);
                        ImGui.Text("Team");
                        ImGui.ColorEdit4("##BoxT", ref boxColorteam);
                        ImGui.Spacing();
                        ImGui.Separator();
                        if (ImGui.TreeNode("Advanced Box Options"))
                        {
                            ImGui.Text("Box Thickness");
                            ImGui.SliderFloat("##BoxThickness", ref boxThickness, 0, 20);

                            ImGui.Text("Box Roundness");
                            ImGui.SliderFloat("##BoxRoundness", ref boxRoundness, 0, 20);

                            ImGui.Text("Top Hight");
                            ImGui.SliderInt("##TopHight", ref TopBoxHeight, -100, 100);

                            ImGui.Text("Top Width");
                            ImGui.SliderInt("##TopWidth", ref TopBoxWidth, -100, 100);

                            ImGui.Text("Bottom Height");
                            ImGui.SliderInt("##BottomHeight", ref BottomBoxHeight, -100, 100);

                            ImGui.Text("Bottom Width");
                            ImGui.SliderInt("##BottomWidth", ref BottomBoxWidth, -100, 100);
                            ImGui.TreePop();
                        }

                        ImGui.TreePop();
                    }

                    ImGui.Spacing();
                    ImGui.Checkbox("Tracklines Enabled", ref EnableTracklines);
                    if (EnableTracklines && ImGui.TreeNode("Tracklines Settings"))
                    {
                        ImGui.Checkbox("Team Trackline Enabled", ref ShowTeamTrackline);
                        ImGui.Checkbox("Trackline Visibility Check", ref NotTSpotted);
                        if (NotTSpotted)
                        {
                            ImGui.Text("Invisible Color");
                            ImGui.ColorEdit4("##Tracks", ref NTS);
                        }
                        ImGui.Text("Enemy");
                        ImGui.ColorEdit4("##TrackE", ref tracklinesColorEnemy);
                        ImGui.Text("Team");
                        ImGui.ColorEdit4("##TrackT", ref tracklinesColorTeam);
                        ImGui.Spacing();
                        ImGui.Separator();
                        if (ImGui.TreeNode("Advanced Trackline Options"))
                        {
                            ImGui.Text("Tracklines Thickness");
                            ImGui.SliderFloat("##TracklinesThickness", ref tracklineThickness, 0, 20);

                            ImGui.Text("Enemy Position X");
                            ImGui.SliderInt("##EnemyPositionX", ref tracklinePosX, -100, 100);

                            ImGui.Text("Enemy Position Y");
                            ImGui.SliderInt("##EnemyPositionY", ref tracklinePosY, -100, 100);

                            ImGui.Text("Screen Position X");
                            ImGui.SliderInt("##ScreenPositionX", ref startXpos, -600, 600);

                            ImGui.Text("Screen Position Y");
                            ImGui.SliderInt("##ScreenPositionY", ref startYpos, -600, 600);

                            ImGui.TreePop();
                        }

                        ImGui.TreePop();
                    }


                    ImGui.Spacing();
                    ImGui.Checkbox("Health Bar Enabled", ref HealthBar);
                    if (HealthBar && ImGui.TreeNode("HealthBar Settings"))
                    {
                        ImGui.Checkbox("Team Health Enabled", ref ShowTeamHealth);


                        ImGui.Text("Enemy Health 100");
                        ImGui.ColorEdit4("##EF100t50", ref healthColorEnemy);

                        ImGui.Text("Team Health 100");
                        ImGui.ColorEdit4("##TF100t50", ref healthColorTeam);
                        ImGui.Spacing();
                        ImGui.Separator();
                        if (ImGui.TreeNode("Advanced Health Esp Settings"))
                        {
                            ImGui.Text("##HealthBarThickness");
                            ImGui.SliderFloat("Health Bar Thickness", ref healthThickness, 0, 20);
                            ImGui.Text("Health Bar Roundness");
                            ImGui.SliderFloat("##HealthBarRoundness", ref healthroundness, 0, 20);

                            ImGui.Text("Enemy Health From 50 to 0");
                            ImGui.ColorEdit4("##EF50t0", ref healthColorE50);

                            ImGui.Text("Team Health From 50 to 0");
                            ImGui.ColorEdit4("##TF50t0", ref healthColorT50);
                            ImGui.TreePop();
                        }

                        ImGui.TreePop();
                    }

                    ImGui.Spacing();
                    ImGui.Checkbox("Bone ESP Enabled", ref BoneEsp);
                    if (BoneEsp && ImGui.TreeNode("Bone Settings"))
                    {
                        ImGui.Checkbox("Team Bones Enabled", ref showTeamBone);
                        ImGui.Checkbox("Bone Visibility Check", ref NotBoneSpotted);
                        if (NotBoneSpotted)
                        {
                            ImGui.Text("Invisible Color");
                            ImGui.ColorEdit4("##Bones", ref NBoneS);
                        }



                        ImGui.Text("Enemy Bone Color");
                        ImGui.ColorEdit4("##bone color", ref BoneColors);
                        ImGui.Text("Team Bone Color");
                        ImGui.ColorEdit4("##team bone color", ref TeamBoneColors);
                        ImGui.Spacing();
                        ImGui.Separator();
                        if (ImGui.TreeNode("Advanced Bone Esp Settings"))
                        {
                            ImGui.Text("Bone Thickness");
                            ImGui.SliderFloat("##BoneThickness", ref boneThickness, 1, 10);

                            ImGui.Checkbox("Render Head", ref onlyRenderBoneHead);
                            ImGui.TreePop();
                        }
                        ImGui.TreePop();
                    }

                    ImGui.Spacing();
                    ImGui.Checkbox("Head ESP Enabled", ref HeadEsps);
                    if (HeadEsps && ImGui.TreeNode("Head Settings"))
                    {
                        ImGui.Checkbox("Team Head Enabled", ref displayTeamHead);
                        ImGui.Checkbox("Head Visibility Check", ref VisibiltyHeadCheck);
                        ImGui.Checkbox("Draw Face", ref addFace);

                        if (NotBoneSpotted)
                        {
                            ImGui.Text("Invisible Color");
                            ImGui.ColorEdit4("##HeadC", ref UnVisibleHeadColor);
                        }

                        ImGui.Text("Team Head Color");
                        ImGui.ColorEdit4("##head b color", ref HeadTeam);
                        ImGui.Text("Enemy Head Color");
                        ImGui.ColorEdit4("##head T color", ref HeadEnemy);
                        ImGui.Spacing();
                        ImGui.Separator();
                        if (ImGui.TreeNode("Advanced Head Esp Settings"))
                        {
                            if (addFace)
                            {
                                ImGui.Text("Face size");
                                ImGui.SliderFloat("##Facesize", ref FaceSize, 0, 100);
                            }
                            ImGui.Text("Head Thickness");
                            ImGui.SliderFloat("##HeadThickness", ref headThickness, 1, 100);
                            ImGui.TreePop();
                        }
                        ImGui.TreePop();
                    }
                    ImGui.Spacing();
                    ImGui.Checkbox("Name ESP Enabled", ref NameEsp);
                    if (NameEsp && ImGui.TreeNode("Name Esp Settings"))
                    {
                        ImGui.Checkbox("Team Names Enabled", ref DisplayTeamName);
                        ImGui.Checkbox("Trackline Visibility Check", ref NotNSpotted);
                        if (NotNSpotted)
                        {
                            ImGui.Text("Invisible Color");
                            ImGui.ColorEdit4("##Names", ref NNS);
                        }

                        ImGui.Text("Enemy");
                        ImGui.ColorEdit4("##enColor", ref NameEspEnemy);
                        ImGui.Text("Team");
                        ImGui.ColorEdit4("##tnColor", ref NameEspTeam);
                        ImGui.Spacing();
                        ImGui.Separator();
                        if (ImGui.TreeNode("Advanced Name Esp Settings"))
                        {
                            ImGui.Text("Name Y Offset");
                            ImGui.SliderInt("##NameYOffset", ref yOffs, 0, 100);
                            ImGui.Text("Name X Offset");
                            ImGui.SliderInt("##NameXOffset", ref xOffs, 0, 100);
                            ImGui.TreePop();
                        }
                        ImGui.TreePop();
                    }

                    ImGui.Spacing();
                    ImGui.Checkbox("Weapon ESP Enabled", ref WeaponESP);
                    if (WeaponESP && ImGui.TreeNode("Weapon Esp Settings"))
                    {
                        ImGui.Checkbox("Team Weapon Enabled", ref DisplayTeamWeapon);


                        ImGui.Text("Enemy");
                        ImGui.ColorEdit4("##WEColor", ref WEECOLOR);
                        ImGui.Text("Team");
                        ImGui.ColorEdit4("WTColor", ref WTTCOLOR);
                        ImGui.Spacing();
                        ImGui.Separator();
                        if (ImGui.TreeNode("Advanced Weapon Esp Settings"))
                        {
                            ImGui.Text("Weapon Y Offset");
                            ImGui.SliderInt("##WeaponYOffset", ref WyOffs, 0, 100);
                            ImGui.Text("Weapon X Offset");
                            ImGui.SliderInt("##WeaponXOffset", ref WxOffs, 0, 100);
                            ImGui.TreePop();
                        }
                        ImGui.TreePop();
                    }

                    ImGui.Spacing();
                    ImGui.Checkbox("Distance ESP Enabled", ref EnableDistanceCheck);
                    if (EnableDistanceCheck && ImGui.TreeNode("Distance Esp Settings"))
                    {
                        ImGui.Checkbox("Team Distance Enabled", ref teanD);
                        ImGui.Text("Enemy");
                        ImGui.ColorEdit4("##WEColor", ref DistanceEnemyColor);
                        ImGui.Text("Team");
                        ImGui.ColorEdit4("WTColor", ref DistanceTeamColor);

                        ImGui.Spacing();
                        ImGui.Separator();
                        if (ImGui.TreeNode("Advanced Distance Esp Settings"))
                        {
                            ImGui.Text("Distance Y Offset");
                            ImGui.SliderInt("##DistanceYOffset", ref DyOffs, 0, 100);
                            ImGui.Text("Distance X Offset");
                            ImGui.SliderInt("##DistanceXOffset", ref DxOffs, 0, 100);
                            ImGui.TreePop();
                        }
                        ImGui.TreePop();
                    }


                    //ImGui.Spacing();
                    //ImGui.Text("Not Recomended While Using Aimbot");
                    //ImGui.Checkbox("Radar Enabled", ref Radar);
                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem("Other"))
                {
                    ImGui.Spacing();
                    ImGui.Checkbox("Ingore Flashbangs", ref NoFlash);
                    ImGui.Spacing();
                    ImGui.Checkbox("Fov Change Enabled", ref FovEnabled);
                    if (FovEnabled)
                    {
                        ImGui.SliderInt("Field of view", ref fov, 1, 180);
                    }
                    ImGui.Spacing();
                    ImGui.Separator();
                    ImGui.Text("Settings");
                    ImGui.Spacing();
                    ImGui.Checkbox("Draw Crosshair", ref showplus);
                    if (showplus)
                    {
                        if (ImGui.TreeNode("Crosshair Customization"))
                        {
                            ImGui.SliderFloat("Size", ref size, 1, 100.0f, "%.1f");
                            ImGui.SliderFloat("Thickness", ref thickness, 0.1f, 20.0f, "%.1f");
                            if (ImGui.CollapsingHeader("Crosshair Color")) ImGui.ColorEdit4("##crosshaircolor", ref color);
                            ImGui.SliderFloat("Outline Thickness", ref outlineThickness, 0.1f, 20.0f, "%.1f");
                            if (ImGui.CollapsingHeader("Outline Color")) ImGui.ColorEdit4("##outlinecolor", ref outlineColor);
                            ImGui.SliderFloat("Center Dot Size", ref centerDotSize, 1.0f, 100.0f, "%.1f");
                            ImGui.SliderFloat("Crosshair Opacity", ref opacity, 0f, 1f, "%.2f");
                            ImGui.TreePop();

                        }
                    }
                    ImGui.Checkbox("Status Overlay", ref EnableStatus);
                    //ImGui.Checkbox("Enable Money Checking", ref enablemoneyChecking);
                    ImGui.Checkbox("Delay", ref LessCpuUsageButLag);
                    if (LessCpuUsageButLag && ImGui.TreeNode("Delay Settings"))
                    {
                        if (ImGui.TreeNode("Delay Info"))
                        {
                            ImGui.Text("Increasing This Will Make Aimbot Slower");
                            ImGui.Text("Increasing This Will Make Esp Slower");
                            ImGui.Text("Increasing This Will Make PC faster");
                            ImGui.Text("1 MS and 2 MS are Recomended Options");
                            ImGui.TreePop();
                        }
                        ImGui.Checkbox("Input Delay Number", ref xx);
                        if (xx)
                        {
                            ImGui.InputInt("Delay in miliseconds", ref delay);
                        }
                        else
                        {
                            ImGui.SliderInt("Delay in miliseconds", ref delay, 0, 1000);
                        }
                        ImGui.TreePop();
                    }
                    ImGui.EndTabItem();
                }


                if (enablemoneyChecking)
                {
                    if (ImGui.BeginTabItem("Money"))
                    {
                        try
                        {
                            ImGui.Text("Money");
                            List<Entity> entitiesCopy = new List<Entity>(nentities).ToList();
                            foreach (Entity entity in entitiesCopy)
                            {
                                if (ImGui.TreeNode(entity._name))
                                {
                                    ImGui.TextColored(greenColor, $"Account: {entity.account}");
                                    ImGui.TextColored(greenColor, $"Cash Spent Now: {entity.cashSpent}");
                                    ImGui.TextColored(greenColor, $"Cash Spent Total: {entity.cashSpentTotal}");
                                    ImGui.TreePop();
                                }
                            }
                        }
                        catch { }

                        ImGui.EndTabItem();
                    }
                }

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
                ImGui.EndTabBar();
            }
            DrawOverlay("overlay");
            _drawlist = ImGui.GetBackgroundDrawList();
            foreach (var entity in espentities)
            {
                if (EntityOnScreen(entity))
                {
                    if (BoxEsp)
                    {
                        DrawBox(entity);
                    }
                    if (EnableTracklines)
                    {
                        DrawLine(entity);
                    }
                    if (HealthBar)
                    {
                        DrawHealthBar(entity);
                    }
                    if (BoneEsp)
                    {
                        DrawBones(entity);
                    }
                    if (NameEsp)
                    {
                        DrawNames(entity);
                    }
                    if (HeadEsps)
                    {
                        HeadESP(entity);
                    }
                    if (EnableDistanceCheck)
                    {
                        DrawDistance(entity);
                    }
                }
            }

            if (WeaponESP) DrawWeapon();

            if (showplus) RenderOverlay();

            if (showFov)
            {
                Vector4 Color = circleColor;
                if (visibleColorChangeFOV)
                {
                    Color = EntityVisible == true ? invisibleColor : Color;
                }
                _drawlist.AddCircle(new Vector2(screenSize.X / 2, screenSize.Y / 2), AimbotFOV, ImGui.ColorConvertFloat4ToU32(Color));
            }
            if (!isRunning) Environment.Exit(-1);

            if (EnableStatus)
            {
                Vector2 size1 = new Vector2(180, 150);
                Vector2 windowPos = ImGui.GetWindowPos();
                Vector2 windowSize = ImGui.GetWindowSize();
                Vector2 position = new Vector2((windowPos.X + windowSize.X - size1.X - 9), (windowPos.Y + 3));

                var drawList = ImGui.GetWindowDrawList();
                drawList.AddRectFilled(position, position + size1, ImGui.ColorConvertFloat4ToU32(new Vector4(32.0f / 255.0f, 32.0f / 255.0f, 32.0f / 255.0f, 0.50f)));

                ImGui.SetCursorPos(position - windowPos); // Set cursor position relative to the current window's origin

                ImGui.BeginChild("RectangleContent", size1 - new Vector2(20, 40));

                ImGui.Text("Speed meter");
                ImGui.TextColored(GetSpeedColor(maxSpeed), $"Max Speed: {maxSpeed}");
                ImGui.TextColored(GetSpeedColor(speed), $"Current Speed: {speed}");
                ImGui.Spacing();
                ImGui.Spacing();

                ImGui.Text("Bomb timer");
                if (bombPlanted)
                {
                    ImGui.TextColored(redColor, "Planted");
                    ImGui.Text($"Blowing In: {timeLeft}");
                }
                else
                {
                    ImGui.TextColored(greenColor, "Not Planted");
                }

                ImGui.EndChild();
            }
        }


        public void DrawNames(Entity entity)
        {
            Vector2 textLocation = new Vector2(entity.viewPosition2D.X - xOffs, entity.viewPosition2D.Y - yOffs);
            Vector4 color = localPlayer.team == entity.team ? NameEspTeam : NameEspEnemy;
            if (NotNSpotted)
            {
                color = entity.spotted == true ? color : NNS;
            }
            if (entity.team == localPlayer.team && DisplayTeamName == true)
            {
                _drawlist.AddText(textLocation, ImGui.ColorConvertFloat4ToU32(color), $"{entity._name}");
            }
            else if (entity.team != localPlayer.team)
            {
                _drawlist.AddText(textLocation, ImGui.ColorConvertFloat4ToU32(color), $"{entity._name}");
            }
        }
        bool EntityOnScreen(Entity entity)
        {
            if (entity.position2D.X > 0 && entity.position2D.X < screenSize.X && entity.position2D.Y > 0 && entity.position2D.Y < screenSize.Y)
            {
                return true;
            }
            return false;
        }
        void DrawOverlay(string name)
        {
            ImGui.SetNextWindowSize(screenSize);
            ImGui.SetWindowPos(new Vector2(0, 0));
            ImGui.Begin(name, ImGuiWindowFlags.NoDecoration
                | ImGuiWindowFlags.NoBackground
                | ImGuiWindowFlags.NoBringToFrontOnFocus
                | ImGuiWindowFlags.NoMove
                | ImGuiWindowFlags.NoInputs
                | ImGuiWindowFlags.NoCollapse
                | ImGuiWindowFlags.NoScrollbar
                | ImGuiWindowFlags.NoScrollWithMouse);

        }
        public void DrawBones(Entity entity)
        {
            uint uintColor = localPlayer.team == entity.team ? ImGui.ColorConvertFloat4ToU32(TeamBoneColors) : ImGui.ColorConvertFloat4ToU32(BoneColors);
            float currentBoneThickness = (boneThickness * 1000) / entity.distance;
            if (NotBoneSpotted)
            {
                uintColor = entity.spotted == true ? uintColor : ImGui.ColorConvertFloat4ToU32(NBoneS);
            }
            bool onTeam = localPlayer.team == entity.team;
            if (onTeam && showTeamBone)
            {
                _drawlist.AddLine(entity.bones2D[1], entity.bones2D[2], uintColor, currentBoneThickness);
                _drawlist.AddLine(entity.bones2D[1], entity.bones2D[3], uintColor, currentBoneThickness);
                _drawlist.AddLine(entity.bones2D[1], entity.bones2D[6], uintColor, currentBoneThickness);
                _drawlist.AddLine(entity.bones2D[3], entity.bones2D[4], uintColor, currentBoneThickness);
                _drawlist.AddLine(entity.bones2D[6], entity.bones2D[7], uintColor, currentBoneThickness);
                _drawlist.AddLine(entity.bones2D[4], entity.bones2D[5], uintColor, currentBoneThickness);
                _drawlist.AddLine(entity.bones2D[7], entity.bones2D[8], uintColor, currentBoneThickness);
                _drawlist.AddLine(entity.bones2D[1], entity.bones2D[0], uintColor, currentBoneThickness);
                _drawlist.AddLine(entity.bones2D[0], entity.bones2D[9], uintColor, currentBoneThickness);
                _drawlist.AddLine(entity.bones2D[0], entity.bones2D[11], uintColor, currentBoneThickness);
                _drawlist.AddLine(entity.bones2D[9], entity.bones2D[10], uintColor, currentBoneThickness);
                _drawlist.AddLine(entity.bones2D[11], entity.bones2D[12], uintColor, currentBoneThickness);
                if (onlyRenderBoneHead)
                {
                    _drawlist.AddCircle(entity.bones2D[2], 3 + currentBoneThickness, uintColor);
                }

            }
            else if (!onTeam)
            {

                _drawlist.AddLine(entity.bones2D[1], entity.bones2D[2], uintColor, currentBoneThickness);
                _drawlist.AddLine(entity.bones2D[1], entity.bones2D[3], uintColor, currentBoneThickness);
                _drawlist.AddLine(entity.bones2D[1], entity.bones2D[6], uintColor, currentBoneThickness);
                _drawlist.AddLine(entity.bones2D[3], entity.bones2D[4], uintColor, currentBoneThickness);
                _drawlist.AddLine(entity.bones2D[6], entity.bones2D[7], uintColor, currentBoneThickness);
                _drawlist.AddLine(entity.bones2D[4], entity.bones2D[5], uintColor, currentBoneThickness);
                _drawlist.AddLine(entity.bones2D[7], entity.bones2D[8], uintColor, currentBoneThickness);
                _drawlist.AddLine(entity.bones2D[1], entity.bones2D[0], uintColor, currentBoneThickness);
                _drawlist.AddLine(entity.bones2D[0], entity.bones2D[9], uintColor, currentBoneThickness);
                _drawlist.AddLine(entity.bones2D[0], entity.bones2D[11], uintColor, currentBoneThickness);
                _drawlist.AddLine(entity.bones2D[9], entity.bones2D[10], uintColor, currentBoneThickness);
                _drawlist.AddLine(entity.bones2D[11], entity.bones2D[12], uintColor, currentBoneThickness);

                if (onlyRenderBoneHead)
                {
                    _drawlist.AddCircle(entity.bones2D[2], 3 + currentBoneThickness, uintColor);
                }
            }
        }
        public void HeadESP(Entity entity)
        {
            uint uintColor = localPlayer.team == entity.team ? ImGui.ColorConvertFloat4ToU32(HeadTeam) : ImGui.ColorConvertFloat4ToU32(HeadEnemy);
            float currentBoneThickness = (headThickness * 1000) / entity.distance;

            if (VisibiltyHeadCheck)
            {
                uintColor = entity.spotted == true ? uintColor : ImGui.ColorConvertFloat4ToU32(UnVisibleHeadColor);
            }

            bool onTeam = localPlayer.team == entity.team;

            if (onTeam && displayTeamHead)
            {
                _drawlist.AddCircle(entity.bones2D[2], 3 + currentBoneThickness, uintColor);

                if (addFace)
                {
                    Vector2 facePosition = entity.bones2D[2];
                    float faceSize = FaceSize;

                    _drawlist.AddCircle(facePosition + new Vector2(-faceSize * 0.3f, -faceSize * 0.3f), faceSize * 0.2f, uintColor);
                    _drawlist.AddCircle(facePosition + new Vector2(faceSize * 0.3f, -faceSize * 0.3f), faceSize * 0.2f, uintColor);

                    _drawlist.AddLine(
                        facePosition + new Vector2(-faceSize * 0.4f, faceSize * 0.3f),
                        facePosition + new Vector2(faceSize * 0.4f, faceSize * 0.3f),
                        uintColor
                    );
                }
            }
            else if (!onTeam)
            {
                _drawlist.AddCircle(entity.bones2D[2], 3 + currentBoneThickness, uintColor);

                if (addFace)
                {
                    Vector2 facePosition = entity.bones2D[2];
                    float faceSize = FaceSize;

                    _drawlist.AddCircle(facePosition + new Vector2(-faceSize * 0.3f, -faceSize * 0.3f), faceSize * 0.2f, uintColor);
                    _drawlist.AddCircle(facePosition + new Vector2(faceSize * 0.3f, -faceSize * 0.3f), faceSize * 0.2f, uintColor);

                    _drawlist.AddLine(
                        facePosition + new Vector2(-faceSize * 0.4f, faceSize * 0.3f),
                        facePosition + new Vector2(faceSize * 0.4f, faceSize * 0.3f),
                        uintColor
                    );
                }
            }


        }

        public void DrawHealthBar(Entity entity)
        {
            float entityHeight = entity.position2D.Y - entity.viewPosition2D.Y;

            float boxLeft = entity.viewPosition2D.X - entityHeight / 3;
            float boxRight = entity.position2D.X + entityHeight / 3;

            float barPercentWidth = healthThickness;
            float barPixelWidth = barPercentWidth * (boxRight - boxLeft);

            float BarHeight = entityHeight * (entity.health / 100f);

            Vector2 barTop = new Vector2(boxLeft - barPercentWidth, entity.position2D.Y - BarHeight);
            Vector2 barBottom = new Vector2(boxLeft, entity.position2D.Y);

            bool sameTeam = localPlayer.team == entity.team ? true : false;

            Vector4 barColor = GetColor(sameTeam, entity.health);

            if (entity.team == localPlayer.team && ShowTeamHealth)
            {
                _drawlist.AddRectFilled(barTop, barBottom, ImGui.ColorConvertFloat4ToU32(barColor), healthroundness);
            }
            else if (entity.team != localPlayer.team)
            {
                _drawlist.AddRectFilled(barTop, barBottom, ImGui.ColorConvertFloat4ToU32(barColor), healthroundness);
            }
        }
        Vector4 GetColor(bool Sameteam, int health)
        {
            if (Sameteam && health >= 50)
            {
                return healthColorTeam;
            }
            else if (Sameteam && health <= 50)
            {
                return healthColorT50;
            }
            else if (!Sameteam && health >= 50)
            {
                return healthColorEnemy;
            }
            else
            {
                return healthColorE50;
            }
        }
        public void DrawLine(Entity entity)
        {
            ImDrawListPtr _drawlist = new ImDrawListPtr();
            _drawlist = ImGui.GetWindowDrawList();
            Vector4 lineColor = localPlayer.team == entity.team ? tracklinesColorTeam : tracklinesColorEnemy;
            if (localPlayer.team == entity.team && !ShowTeamTrackline)
                return;
            if (NotTSpotted)
            {
                lineColor = entity.spotted == true ? lineColor : NTS;
            }

            Vector2 ent = new Vector2(entity.position2D.X - tracklinePosX, entity.position2D.Y - tracklinePosY);
            _drawlist.AddLine(new Vector2((screenSize.X / 2) - startXpos, (screenSize.Y / 2) - startYpos), ent, ImGui.ColorConvertFloat4ToU32(lineColor), tracklineThickness);
        }
        public void DrawBox(Entity entity)
        {
            float entityHeight = entity.position2D.Y - entity.viewPosition2D.Y;
            Vector2 rectTop = new Vector2((entity.viewPosition2D.X - entityHeight / 3) - TopBoxWidth, (entity.viewPosition2D.Y) - TopBoxHeight);
            Vector2 rectBottom = new Vector2((entity.position2D.X + entityHeight / 3) - BottomBoxWidth, entity.position2D.Y - BottomBoxHeight);
            Vector4 boxColor = localPlayer.team == entity.team ? boxColorteam : boxColorEnemy;

            if (NotBSpotted)
            {
                boxColor = entity.spotted == true ? boxColor : NBS;
            }
            if (localPlayer.team == entity.team && ShowTeamBox)
                _drawlist.AddRect(rectTop, rectBottom, ImGui.ColorConvertFloat4ToU32(boxColor), boxRoundness, ImDrawFlags.None, boxThickness);
            else if (localPlayer.team != entity.team)
                _drawlist.AddRect(rectTop, rectBottom, ImGui.ColorConvertFloat4ToU32(boxColor), boxRoundness, ImDrawFlags.None, boxThickness);
        }
        public void UpdateLocalPlayer(Entity newEntity)
        {
            lock (entityLock)
            {
                localPlayer = newEntity;
            }
        }
        public void UpdateEntities(IEnumerable<Entity> newEntities)
        {
            espentities = new ConcurrentQueue<Entity>(newEntities);
        }
        private void RenderOverlay()
        {
            var viewport = ImGui.GetMainViewport();
            var centerX = viewport.Size.X / 2 + viewport.Pos.X;
            var centerY = viewport.Size.Y / 2 + viewport.Pos.Y;

            // Adjust color with opacity
            uint drawColor = GetColorWithOpacity(color, opacity);
            uint drawOutlineColor = GetColorWithOpacity(outlineColor, opacity);

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
                drawColor, thickness);

            // Draw the horizontal line
            _drawlist.AddLine(
                new Vector2(centerX - size / 2, centerY),
                new Vector2(centerX + size / 2, centerY),
                drawColor, thickness);

            // Draw center dot
            _drawlist.AddCircleFilled(
                new Vector2(centerX, centerY), centerDotSize, drawColor);
        }

        void DrawDistance(Entity entity)
        {
            Vector2 textLocation = new Vector2(entity.viewPosition2D.X - DxOffs, entity.viewPosition2D.Y - DyOffs);
            Vector4 color = localPlayer.team == entity.team ? DistanceTeamColor : DistanceEnemyColor;
            if (entity.team == localPlayer.team && teanD)
            {
                _drawlist.AddText(textLocation, ImGui.ColorConvertFloat4ToU32(color), $"{(int)entity.distance / 100}m");
            }
            else if (entity.team != localPlayer.team)
            {
                _drawlist.AddText(textLocation, ImGui.ColorConvertFloat4ToU32(color), $"{(int)entity.distance / 100}m");
            }
        }
        void DrawWeapon()
        {
            try
            {
                List<Entity> tempEntities = new List<Entity>(entities).ToList();
                foreach (Entity entity in tempEntities)
                {
                    if (entity != null)
                    {
                        Vector2 textLocation = new Vector2(entity.position2D.X - WxOffs, entity.position2D.Y - WyOffs);
                        if (localPlayer.team != entity.team)
                        {
                            _drawlist.AddText(textLocation, ImGui.ColorConvertFloat4ToU32(WEECOLOR), $"{entity.currentWeaponName}");
                        }
                        else if (localPlayer.team == entity.team && DisplayTeamWeapon)
                        {
                            _drawlist.AddText(textLocation, ImGui.ColorConvertFloat4ToU32(WTTCOLOR), $"{entity.currentWeaponName}");
                        }
                    }
                }
            }
            catch { }

        }

        Vector4 GetSpeedColor(int speed)
        {
            if (speed > 500)
                return redColor;
            if (speed > 400)
                return yellowColor;
            if (speed > 300)
                return greenColor;
            return white;
        }
        private uint GetColorWithOpacity(Vector4 color, float opacity)
        {
            return ((uint)(opacity * 255) << 24) |  // Alpha (opacity)
                   ((uint)(color.Z * 255) << 16) |  // Blue
                   ((uint)(color.Y * 255) << 8) |  // Green
                   ((uint)(color.X * 255));         // Red
        }
    }
}