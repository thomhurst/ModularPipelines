using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "repositories", "list")]
public record GcloudArtifactsRepositoriesListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}