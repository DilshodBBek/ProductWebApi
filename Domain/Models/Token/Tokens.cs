using System.Text.Json.Serialization;

namespace Domain.Models.Token;

public class Tokens
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }
}
