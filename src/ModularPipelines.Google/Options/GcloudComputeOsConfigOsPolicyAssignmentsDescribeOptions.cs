using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "os-config", "os-policy-assignments", "describe")]
public record GcloudComputeOsConfigOsPolicyAssignmentsDescribeOptions(
[property: CliArgument] string OsPolicyAssignment,
[property: CliArgument] string Location
) : GcloudOptions;