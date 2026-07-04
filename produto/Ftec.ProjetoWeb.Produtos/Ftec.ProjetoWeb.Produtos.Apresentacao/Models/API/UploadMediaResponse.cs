using System.Text.Json.Serialization;

public class UploadMediaResponse
{
    [JsonPropertyName("sucesso")]
    public bool Sucesso { get; set; }

    [JsonPropertyName("data")]
    public MediaUploadModel Data { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}