using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vm", "repair", "list-scripts")]
public record AzVmRepairListScriptsOptions : AzOptions
{
    [CliOption("--preview")]
    public string? Preview { get; set; }
}