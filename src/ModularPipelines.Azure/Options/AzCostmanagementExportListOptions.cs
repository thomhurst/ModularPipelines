using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("costmanagement", "export", "list")]
public record AzCostmanagementExportListOptions(
[property: CliOption("--scope")] string Scope
) : AzOptions;