using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Domain.Models;

public class Response
{
    [JsonPropertyName("status_code")]
    public int StatusCode { get; set; } = 200;
    [JsonPropertyOrder(0)]
    public string Message { get; set; } = "";
    public bool IsSuccess { get; set; }=true;
    public object Result { get; set; }

    public int page { get; set; } = 1;
    public int pageSize { get; set; } = 10;
}
