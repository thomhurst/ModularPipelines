using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workloads", "sap-database-instance", "list")]
public record AzWorkloadsSapDatabaseInstanceListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sap-virtual-instance-name")] string SapVirtualInstanceName
) : AzOptions;