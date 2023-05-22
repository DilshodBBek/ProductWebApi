using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Token;

[Table("refresh_token")]
public class RefreshToken
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("refresh_token_id")]
    public int RefreshTokenId { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    [Column("refresh_token_value")]
    public string RefreshTokenValue { get; set; }

    [Column("expired_date")]
    public DateTime ExpiredDate { get; set; }
}
