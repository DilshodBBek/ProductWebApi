using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Models;

[Table("role")]
public class Role
{
    [Column("role_id")]
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonPropertyName("role_id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [JsonIgnore]
    public virtual ICollection<RolePermission>? RolePermissions { get; set; }
    [JsonIgnore]
    public virtual ICollection<UserRole>? UserRoles { get; set; }

    [JsonPropertyName("Permission_names")]
    [NotMapped]
    public int[] Permissionids { get; set; }
}
