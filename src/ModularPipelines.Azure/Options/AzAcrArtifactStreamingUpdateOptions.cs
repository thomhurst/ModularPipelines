using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "artifact-streaming", "update")]
public record AzAcrArtifactStreamingUpdateOptions(
[property: CliFlag("--enable-streaming")] bool EnableStreaming,
[property: CliOption("--name")] string Name,
[property: CliOption("--repository")] string Repository
) : AzOptions
{
    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--suffix")]
    public string? Suffix { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }
}