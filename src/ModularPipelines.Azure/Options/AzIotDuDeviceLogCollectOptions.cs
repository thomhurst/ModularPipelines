using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "du", "device", "log", "collect")]
public record AzIotDuDeviceLogCollectOptions(
[property: CommandSwitch("--account")] int Account,
[property: CommandSwitch("--agent-id")] string AgentId,
[property: CommandSwitch("--instance")] string Instance,
[property: CommandSwitch("--lcid")] string Lcid
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}