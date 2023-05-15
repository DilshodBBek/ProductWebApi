using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Models;

[Table("product")]
public class Product
{
    [Column("product_id")]
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonPropertyName("product_id")]
    public int ProductId { get; set; }
    [Column("name")]
    public string Name { get; set; }
    [Column("description")]
    public string Description { get; set; }
    [Column("price")]
    public decimal Price { get; set; }
}
