using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "os-config", "os-policy-assignments", "operations", "describe")]
public record GcloudComputeOsConfigOsPolicyAssignmentsOperationsDescribeOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Location,
[property: CliArgument] string OsPolicyAssignment
) : GcloudOptions;