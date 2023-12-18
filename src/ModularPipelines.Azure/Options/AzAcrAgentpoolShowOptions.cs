using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "agentpool", "show")]
public record AzAcrAgentpoolShowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--registry")] string Registry
) : AzOptions
{
    [BooleanCommandSwitch("--queue-count")]
    public bool? QueueCount { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}