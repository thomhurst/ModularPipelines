using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("automanage", "configuration-profile-assignment", "arc", "report", "list")]
public record AzAutomanageConfigurationProfileAssignmentArcReportListOptions(
[property: CliOption("--assignment-name")] string AssignmentName,
[property: CliOption("--machine-name")] string MachineName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;