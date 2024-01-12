using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "os-config", "os-policy-assignments", "operations", "describe")]
public record GcloudComputeOsConfigOsPolicyAssignmentsOperationsDescribeOptions(
[property: PositionalArgument] string Operation,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string OsPolicyAssignment
) : GcloudOptions;