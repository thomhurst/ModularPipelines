using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "disks", "remove-labels")]
public record GcloudComputeDisksRemoveLabelsOptions(
[property: CliArgument] string DiskName,
[property: CliFlag("--all")] bool All,
[property: CliOption("--labels")] string[] Labels
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}