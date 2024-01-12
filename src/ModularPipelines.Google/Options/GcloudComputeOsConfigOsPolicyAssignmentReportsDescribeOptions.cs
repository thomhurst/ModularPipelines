using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "os-config", "os-policy-assignment-reports", "describe")]
public record GcloudComputeOsConfigOsPolicyAssignmentReportsDescribeOptions(
[property: PositionalArgument] string InstanceOsPolicyAssignment,
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Location
) : GcloudOptions;