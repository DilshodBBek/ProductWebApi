using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Models;

[Table("permission")]
public class Permission
{
    [Column("permission_id")]
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonPropertyName("permission_id")]
    public int PermissionId { get; set; }

    [Column("permission_name")]
    [JsonPropertyName("permission_name")]
    public string? PermissionName { get; set; }

    [JsonIgnore]
    public virtual ICollection<RolePermission>? RolePermissions { get; set; }
}
