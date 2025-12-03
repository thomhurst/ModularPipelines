using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acr", "artifact-streaming", "show")]
public record AzAcrArtifactStreamingShowOptions(
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