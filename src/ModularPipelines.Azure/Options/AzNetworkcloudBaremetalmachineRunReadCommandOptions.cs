using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud", "baremetalmachine", "run-read-command")]
public record AzNetworkcloudBaremetalmachineRunReadCommandOptions(
[property: CommandSwitch("--commands")] string Commands,
[property: CommandSwitch("--limit-time-seconds")] string LimitTimeSeconds
) : AzOptions
{
    [CommandSwitch("--bare-metal-machine-name")]
    public string? BareMetalMachineName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--output-directory")]
    public string? OutputDirectory { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}