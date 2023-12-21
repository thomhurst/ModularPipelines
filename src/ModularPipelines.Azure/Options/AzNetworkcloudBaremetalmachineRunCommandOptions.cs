using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud", "baremetalmachine", "run-command")]
public record AzNetworkcloudBaremetalmachineRunCommandOptions(
[property: CommandSwitch("--limit-time-seconds")] string LimitTimeSeconds,
[property: CommandSwitch("--script")] string Script
) : AzOptions
{
    [CommandSwitch("--arguments")]
    public string? NetworkcloudBaremetalmachineRunCommandArguments { get; set; }

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
    public new string? Subscription { get; set; }
}