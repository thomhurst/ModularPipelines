using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "dps", "linked-hub", "show")]
public record AzIotDpsLinkedHubShowOptions(
[property: CommandSwitch("--dps-name")] string DpsName,
[property: CommandSwitch("--linked-hub")] string LinkedHub
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}