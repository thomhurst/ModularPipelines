using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "hybrid-connection", "remove")]
public record AzFunctionappHybridConnectionRemoveOptions(
[property: CommandSwitch("--hybrid-connection")] string HybridConnection,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--namespace")] string Namespace,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--slot")]
    public string? Slot { get; set; }
}

