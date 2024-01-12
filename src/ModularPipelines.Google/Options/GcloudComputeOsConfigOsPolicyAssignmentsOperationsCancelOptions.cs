using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "os-config", "os-policy-assignments", "operations", "cancel")]
public record GcloudComputeOsConfigOsPolicyAssignmentsOperationsCancelOptions(
[property: PositionalArgument] string Operation,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string OsPolicyAssignment
) : GcloudOptions;