using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "agent-pools", "update")]
public record GcloudTransferAgentPoolsUpdateOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--bandwidth-limit")]
    public string? BandwidthLimit { get; set; }

    [BooleanCommandSwitch("--clear-bandwidth-limit")]
    public bool? ClearBandwidthLimit { get; set; }

    [BooleanCommandSwitch("--clear-display-name")]
    public bool? ClearDisplayName { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }
}