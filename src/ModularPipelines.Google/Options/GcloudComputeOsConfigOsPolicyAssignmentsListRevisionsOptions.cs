using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "os-config", "os-policy-assignments", "list-revisions")]
public record GcloudComputeOsConfigOsPolicyAssignmentsListRevisionsOptions(
[property: PositionalArgument] string OsPolicyAssignment,
[property: PositionalArgument] string Location
) : GcloudOptions;