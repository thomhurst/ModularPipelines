using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("automanage", "configuration-profile-assignment", "cluster", "report", "list")]
public record AzAutomanageConfigurationProfileAssignmentClusterReportListOptions(
[property: CliOption("--assignment-name")] string AssignmentName,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;