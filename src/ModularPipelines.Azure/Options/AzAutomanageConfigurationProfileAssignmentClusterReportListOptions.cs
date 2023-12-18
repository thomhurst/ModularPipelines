using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("automanage", "configuration-profile-assignment", "cluster", "report", "list")]
public record AzAutomanageConfigurationProfileAssignmentClusterReportListOptions(
[property: CommandSwitch("--assignment-name")] string AssignmentName,
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;