using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "agent-pools", "create")]
public record GcloudTransferAgentPoolsCreateOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [BooleanCommandSwitch("--no-async")]
    public bool? NoAsync { get; set; }

    [CommandSwitch("--bandwidth-limit")]
    public string? BandwidthLimit { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }
}