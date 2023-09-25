using System.Text.Json.Serialization;

namespace ModularPipelines.Build;

public class GitHubArtifacts
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("archive_download_url")]
    public Uri? DownloadUrl { get; init; }
}