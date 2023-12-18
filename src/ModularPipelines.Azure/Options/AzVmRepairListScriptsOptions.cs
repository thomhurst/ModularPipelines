using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "repair", "list-scripts")]
public record AzVmRepairListScriptsOptions : AzOptions
{
    [CommandSwitch("--preview")]
    public string? Preview { get; set; }
}