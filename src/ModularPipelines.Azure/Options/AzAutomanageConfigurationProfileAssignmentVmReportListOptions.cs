using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("automanage", "configuration-profile-assignment", "vm", "report", "list")]
public record AzAutomanageConfigurationProfileAssignmentVmReportListOptions(
[property: CliOption("--assignment-name")] string AssignmentName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vm-name")] string VmName
) : AzOptions;