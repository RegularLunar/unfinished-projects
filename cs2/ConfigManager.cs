using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace CS2
{
    public static class ConfigManager
    {
        private static string configDirectory = "configs";

        public static void SaveConfig(string configName, Config config)
        {
            try
            {
                if (!Directory.Exists(configDirectory))
                {
                    Directory.CreateDirectory(configDirectory);
                }
                string sanitizedConfigName = FileNameUtils.SanitizeFileName(configName);
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters = { new Vector4JsonConverter() }
                };
                string jsonString = JsonSerializer.Serialize(config, options);
                File.WriteAllText(Path.Combine(configDirectory, sanitizedConfigName + ".json"), jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving config: {ex.Message}");
            }
        }

        public static Config LoadConfig(string configName)
        {
            try
            {
                string sanitizedConfigName = FileNameUtils.SanitizeFileName(configName);
                string filePath = Path.Combine(configDirectory, sanitizedConfigName + ".json");

                if (File.Exists(filePath))
                {
                    var options = new JsonSerializerOptions
                    {
                        Converters = { new Vector4JsonConverter() }
                    };
                    string jsonString = File.ReadAllText(filePath);
                    return JsonSerializer.Deserialize<Config>(jsonString, options);
                }
                return new Config();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading config: {ex.Message}");
                return new Config();
            }
        }

        public static List<string> GetConfigList()
        {
            if (!Directory.Exists(configDirectory))
            {
                return new List<string>();
            }

            var files = Directory.GetFiles(configDirectory, "*.json");
            var configNames = new List<string>();
            foreach (var file in files)
            {
                configNames.Add(Path.GetFileNameWithoutExtension(file));
            }
            return configNames;
        }
    }

    public class Vector4JsonConverter : JsonConverter<Vector4>
    {
        public override Vector4 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read();
                float x = reader.GetSingle();
                reader.Read();
                float y = reader.GetSingle();
                reader.Read();
                float z = reader.GetSingle();
                reader.Read();
                float w = reader.GetSingle();
                reader.Read();
                return new Vector4(x, y, z, w);
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Vector4 value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.X);
            writer.WriteNumberValue(value.Y);
            writer.WriteNumberValue(value.Z);
            writer.WriteNumberValue(value.W);
            writer.WriteEndArray();
        }
    }

    public static class FileNameUtils
    {
        private static readonly Regex InvalidFileNameChars = new Regex(
            $"[{Regex.Escape(new string(Path.GetInvalidFileNameChars()))}]");

        public static string SanitizeFileName(string fileName)
        {
            return InvalidFileNameChars.Replace(fileName, "Invalid_Name");
        }
    }

    public class Config
    {
        public bool spectatorsEnabled { get; set; }
        public string ConfigName { get; set; }
        public bool AimbotEnabled { get; set; }
        public bool AimOnTeam { get; set; }
        public bool AimOnlyAtSpotted { get; set; }
        public bool FlashCheckAim { get; set; }
        public bool InputFloatSm { get; set; }
        public float Smoothness { get; set; }
        public int ABIndex { get; set; }
        public bool ShowFov { get; set; }
        public float AimbotFOV { get; set; }
        public bool VisibleColorChangeFOV { get; set; }
        public Vector4 InvisibleColor { get; set; }
        public Vector4 CircleColor { get; set; }
        public bool Triggerbot { get; set; }
        public bool FlashCheckTrigger { get; set; }
        public int TBIndex { get; set; }
        public bool AutoBHOP { get; set; }
        public int Bob { get; set; }
        public int BHIndex { get; set; }
        public bool ShootOnJumpShot { get; set; }
        public int AJIndex { get; set; }
        public bool BoxEsp { get; set; }
        public bool ShowTeamBox { get; set; }
        public bool NotBSpotted { get; set; }
        public Vector4 NBS { get; set; }
        public float BoxThickness { get; set; }
        public float BoxRoundness { get; set; }
        public int TopBoxHeight { get; set; }
        public int TopBoxWidth { get; set; }
        public int BottomBoxHeight { get; set; }
        public int BottomBoxWidth { get; set; }
        public Vector4 BoxColorEnemy { get; set; }
        public Vector4 BoxColorTeam { get; set; }
        public bool EnableTracklines { get; set; }
        public bool ShowTeamTrackline { get; set; }
        public bool NotTSpotted { get; set; }
        public Vector4 NTS { get; set; }
        public int TracklinePosX { get; set; }
        public int TracklinePosY { get; set; }
        public int StartXPos { get; set; }
        public int StartYPos { get; set; }
        public float TracklineThickness { get; set; }
        public Vector4 TracklinesColorEnemy { get; set; }
        public Vector4 TracklinesColorTeam { get; set; }
        public bool HealthBar { get; set; }
        public bool ShowTeamHealth { get; set; }
        public float HealthThickness { get; set; }
        public float HealthRoundness { get; set; }
        public Vector4 HealthColorEnemy { get; set; }
        public Vector4 HealthColorE50 { get; set; }
        public Vector4 HealthColorTeam { get; set; }
        public Vector4 HealthColorT50 { get; set; }
        public bool BoneEsp { get; set; }
        public bool ShowTeamBone { get; set; }
        public bool NotBoneSpotted { get; set; }
        public bool OnlyRenderBoneHead { get; set; }
        public Vector4 NBoneS { get; set; }
        public float BoneThickness { get; set; }
        public Vector4 BoneColors { get; set; }
        public Vector4 TeamBoneColors { get; set; }
        public bool HeadEsps { get; set; }
        public bool DisplayTeamHead { get; set; }
        public bool VisibilityHeadCheck { get; set; }
        public bool AddFace { get; set; }
        public float FaceSize { get; set; }
        public Vector4 UnVisibleHeadColor { get; set; }
        public float HeadThickness { get; set; }
        public Vector4 HeadTeam { get; set; }
        public Vector4 HeadEnemy { get; set; }
        public bool NameEsp { get; set; }
        public bool DisplayTeamName { get; set; }
        public bool NotNSpotted { get; set; }
        public Vector4 NNS { get; set; }
        public int YOffs { get; set; }
        public int XOffs { get; set; }
        public Vector4 NameEspEnemy { get; set; }
        public Vector4 NameEspTeam { get; set; }
        public bool WeaponESP { get; set; }
        public bool DisplayTeamWeapon { get; set; }
        public int WyOffs { get; set; }
        public int WxOffs { get; set; }
        public Vector4 WEECOLOR { get; set; }
        public Vector4 WTTCOLOR { get; set; }
        public bool EnableDistanceCheck { get; set; }
        public bool TeanD { get; set; }
        public int DyOffs { get; set; }
        public int DxOffs { get; set; }
        public Vector4 DistanceEnemyColor { get; set; }
        public Vector4 DistanceTeamColor { get; set; }
        public bool Radar { get; set; }
        public bool NoFlash { get; set; }
        public bool FovEnabled { get; set; }
        public int Fov { get; set; }
        public bool ShowPlus { get; set; }
        public float Size { get; set; }
        public float Thickness { get; set; }
        public Vector4 Color { get; set; }
        public float OutlineThickness { get; set; }
        public Vector4 OutlineColor { get; set; }
        public float CenterDotSize { get; set; }
        public float Opacity { get; set; }
        public bool EnableStatus { get; set; }
        public bool EnableMoneyChecking { get; set; }
        public bool LessCpuUsageButLag { get; set; }
        public bool XX { get; set; }
        public int Delay { get; set; }
    }
}

