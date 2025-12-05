using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "remove-labels")]
public record GcloudComputeInstancesRemoveLabelsOptions(
[property: CliArgument] string InstanceName,
[property: CliFlag("--all")] bool All,
[property: CliOption("--labels")] string[] Labels
) : GcloudOptions
{
    [CliOption("--zone")]
    public string? Zone { get; set; }
}