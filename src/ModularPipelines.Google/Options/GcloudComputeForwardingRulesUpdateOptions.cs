using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "forwarding-rules", "update")]
public record GcloudComputeForwardingRulesUpdateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliFlag("--allow-global-access")]
    public bool? AllowGlobalAccess { get; set; }

    [CliFlag("--allow-psc-global-access")]
    public bool? AllowPscGlobalAccess { get; set; }

    [CliOption("--source-ip-ranges")]
    public string[]? SourceIpRanges { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}