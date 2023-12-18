using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "repair", "list-scripts")]
public record AzVmRepairListScriptsOptions : AzOptions
{
    [CommandSwitch("--preview")]
    public string? Preview { get; set; }
}