using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkcloud", "baremetalmachine", "run-read-command")]
public record AzNetworkcloudBaremetalmachineRunReadCommandOptions(
[property: CliOption("--commands")] string Commands,
[property: CliOption("--limit-time-seconds")] string LimitTimeSeconds
) : AzOptions
{
    [CliOption("--bare-metal-machine-name")]
    public string? BareMetalMachineName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--output-directory")]
    public string? OutputDirectory { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}