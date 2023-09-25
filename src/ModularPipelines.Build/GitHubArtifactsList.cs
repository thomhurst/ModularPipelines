using System.Text.Json.Serialization;

namespace ModularPipelines.Build;

public class GitHubArtifactsList
{
    [JsonPropertyName("artifacts")]
    public List<GitHubArtifacts>? Artifacts { get; init; }
}