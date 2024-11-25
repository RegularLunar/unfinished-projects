using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace ConfigSys
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
            return InvalidFileNameChars.Replace(fileName, "INVALID_NAME");
        }
    }

    public class Config
    {
        #region Unlisted
        public bool ShowMenu { get; set; }
        public bool fovChanger { get; set; }
        public int Fov { get; set; }
        public bool NoRecoil { get; set; }
        public bool AutoBhop { get; set; }
        public bool ShowSpeed { get; set; }
        #endregion

        #region Crosshair
        public bool Crosshair { get; set; }
        public float Csize { get; set; }
        public float Thickness { get; set; }
        public float OutlineThickness { get; set; }
        public float Opacity { get; set; }
        public float CircleOpacity { get; set; }

        // Default values (if needed)
        public Vector4 CrosshairColor { get; set; }
        public Vector4 CircleColor { get; set; }
        public Vector4 OutlineColor { get; set; }
        #endregion

        #region ESP
        public bool Esp { get; set; }
        public bool HeadESP { get; set; }
        public bool BoneESP { get; set; }
        public bool BoxESP { get; set; }
        public bool CornerESP { get; set; }
        public bool HealthBarESP { get; set; }
        public bool HealthTextESP { get; set; }
        public bool TrajectoryESP { get; set; }
        public bool NameESP { get; set; }
        public bool WeaponESP { get; set; }
        public bool DistanceESP { get; set; }
        public bool ShipESP { get; set; }
        public bool GruntESP { get; set; }
        public bool ReaperESP { get; set; }
        public bool StalkerESP { get; set; }
        public bool SuicideSpectreESP { get; set; }
        public bool TickESP { get; set; }
        public bool MrvnESP { get; set; }
        public bool PilotESP { get; set; }
        public bool TitanESP { get; set; }
        public bool AiESP { get; set; }
        #endregion

        #region Aimbot
        public bool Aimbot { get; set; }
        public bool EnableAllAimbot { get; set; }
        public bool PilotAimbot { get; set; }
        public bool TitanAimbot { get; set; }
        public bool AiAimbot { get; set; }
        public bool PredictionAimbot { get; set; }
        public float AimbotSmoothing { get; set; }
        public float AimbotFovCircle { get; set; }
        public bool ShowAimbotFovCircle { get; set; }
        public bool Triggerbot { get; set; }
        public int TriggerbotDelay { get; set; }
        public bool AimAtHead { get; set; }
        public bool AimAtNearest { get; set; }
        public bool AimAtBody { get; set; }
        public bool AimAtTitanCrit { get; set; }
        public bool AimAtTitanNearest { get; set; }
        public bool AimAtTitanBody { get; set; }
        #endregion
    }
}