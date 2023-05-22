using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Application.DTOs;

public class UserDTO
{
    [JsonPropertyName("user_id")]
    public int UsersId { get; set; }

    
    public string Username { get; set; }

    [DataType(DataType.Password)]
    [PasswordPropertyText()]
    public string Password { get; set; }


    [Compare("Password")]
    public string ConfirmPassword { get; set; }

    public int[] Roles { get; set; } = new int[1] { 1 };
}
