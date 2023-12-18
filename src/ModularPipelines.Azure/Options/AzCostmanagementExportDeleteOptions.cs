using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("costmanagement", "export", "delete")]
public record AzCostmanagementExportDeleteOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--scope")] string Scope
) : AzOptions
{
    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}