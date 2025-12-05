using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "agent-pools", "update")]
public record GcloudTransferAgentPoolsUpdateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--bandwidth-limit")]
    public string? BandwidthLimit { get; set; }

    [CliFlag("--clear-bandwidth-limit")]
    public bool? ClearBandwidthLimit { get; set; }

    [CliFlag("--clear-display-name")]
    public bool? ClearDisplayName { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }
}