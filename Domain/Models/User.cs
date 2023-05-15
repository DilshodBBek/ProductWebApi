using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Models;

[Table("user")]

public class User
{
    [Column("user_id")]
    [JsonPropertyName("user_id")]
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UsersId { get; set; }

    [Column("username")]
    public string Username { get; set; }

    [Column("password")]
    public string Password { get; set; }

    [JsonIgnore]
    public virtual ICollection<UserRole>? UserRoles { get; set; }

    [NotMapped]
    public int[] Roles { get; set; } = new int[1] { 1 };
}
