using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("automanage", "configuration-profile-assignment", "vm", "report", "list")]
public record AzAutomanageConfigurationProfileAssignmentVmReportListOptions(
[property: CommandSwitch("--assignment-name")] string AssignmentName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vm-name")] string VmName
) : AzOptions;