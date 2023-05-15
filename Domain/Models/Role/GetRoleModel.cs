using System.Text.Json.Serialization;

namespace Domain.Models;

public class GetRoleModel
{
    [JsonPropertyName("role_id")]
    public int Id { get; set; }

    public string Name { get; set; }

    public Permission[] Permissions { get; set; }
}
