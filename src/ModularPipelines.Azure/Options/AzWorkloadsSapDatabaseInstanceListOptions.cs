using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workloads", "sap-database-instance", "list")]
public record AzWorkloadsSapDatabaseInstanceListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sap-virtual-instance-name")] string SapVirtualInstanceName
) : AzOptions;