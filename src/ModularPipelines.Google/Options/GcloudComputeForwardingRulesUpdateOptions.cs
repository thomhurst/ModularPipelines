using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "forwarding-rules", "update")]
public record GcloudComputeForwardingRulesUpdateOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [BooleanCommandSwitch("--allow-global-access")]
    public bool? AllowGlobalAccess { get; set; }

    [BooleanCommandSwitch("--allow-psc-global-access")]
    public bool? AllowPscGlobalAccess { get; set; }

    [CommandSwitch("--source-ip-ranges")]
    public string[]? SourceIpRanges { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}