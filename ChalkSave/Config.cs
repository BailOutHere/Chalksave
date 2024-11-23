using System.Text.Json.Serialization;

namespace ChalkImageDraw;

public class Config {
    [JsonInclude] public bool SomeSetting = true;
}
