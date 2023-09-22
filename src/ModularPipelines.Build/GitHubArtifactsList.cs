using System.Text.Json.Serialization;

namespace ModularPipelines.Build;

public class GitHubArtifactsList
{
    [JsonPropertyName("artifacts")]
    public List<GitHubArtifacts> Artifacts { get; init; }
}

public class GitHubArtifacts
{
    [JsonPropertyName("id")]
    public string Id { get; init; }
    
    [JsonPropertyName("name")]
    public string Name { get; init; }
    
    [JsonPropertyName("archive_download_url")]
    public Uri DownloadUrl { get; init; }
}