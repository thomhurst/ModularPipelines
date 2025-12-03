using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workloads", "sap-application-server-instance", "list")]
public record AzWorkloadsSapApplicationServerInstanceListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sap-virtual-instance-name")] string SapVirtualInstanceName
) : AzOptions;