using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Domain.Models.UserCredentials;

public class UserCredential
{
    [JsonPropertyName("user_name")]
    public string UserName { get; set; }

    [PasswordPropertyText]
    public string Password { get; set; }
}