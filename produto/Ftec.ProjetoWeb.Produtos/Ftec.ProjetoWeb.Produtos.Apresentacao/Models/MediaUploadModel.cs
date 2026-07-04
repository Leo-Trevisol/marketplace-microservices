using System.Text.Json.Serialization;

public class MediaUploadModel
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("nomeArquivo")]
    public string NomeArquivo { get; set; }

    [JsonPropertyName("nomeUnico")]
    public string NomeUnico { get; set; }

    [JsonPropertyName("caminhoArquivo")]
    public string CaminhoArquivo { get; set; }

    [JsonPropertyName("extensao")]
    public string Extensao { get; set; }

    [JsonPropertyName("tipoArquivo")]
    public string TipoArquivo { get; set; }
}