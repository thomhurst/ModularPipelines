using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "agent-pools", "create")]
public record GcloudTransferAgentPoolsCreateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliFlag("--no-async")]
    public bool? NoAsync { get; set; }

    [CliOption("--bandwidth-limit")]
    public string? BandwidthLimit { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }
}