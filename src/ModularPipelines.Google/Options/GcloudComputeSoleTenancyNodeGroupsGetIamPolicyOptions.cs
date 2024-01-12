using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "sole-tenancy", "node-groups", "get-iam-policy")]
public record GcloudComputeSoleTenancyNodeGroupsGetIamPolicyOptions(
[property: PositionalArgument] string NodeGroup,
[property: PositionalArgument] string Zone
) : GcloudOptions;