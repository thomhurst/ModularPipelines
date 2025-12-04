using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("lab", "artifact", "list")]
public record AzLabArtifactListOptions(
[property: CliOption("--artifact-source-name")] string ArtifactSourceName,
[property: CliOption("--lab-name")] string LabName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--expand")]
    public string? Expand { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--orderby")]
    public string? Orderby { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}