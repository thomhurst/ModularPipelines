using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("costmanagement", "export", "list")]
public record AzCostmanagementExportListOptions(
[property: CommandSwitch("--scope")] string Scope
) : AzOptions;