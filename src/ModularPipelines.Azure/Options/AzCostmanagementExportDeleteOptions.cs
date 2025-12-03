using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("costmanagement", "export", "delete")]
public record AzCostmanagementExportDeleteOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--scope")] string Scope
) : AzOptions
{
    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}