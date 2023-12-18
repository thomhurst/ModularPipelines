using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectedmachine", "run-command", "list")]
public record AzConnectedmachineRunCommandListOptions(
[property: CommandSwitch("--machine-name")] string MachineName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--expand")]
    public string? Expand { get; set; }
}

