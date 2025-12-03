using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("costmanagement", "export", "show")]
public record AzCostmanagementExportShowOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--scope")] string Scope
) : AzOptions;