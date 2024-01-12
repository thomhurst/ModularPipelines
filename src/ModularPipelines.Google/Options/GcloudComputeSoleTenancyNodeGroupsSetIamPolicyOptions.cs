using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "sole-tenancy", "node-groups", "set-iam-policy")]
public record GcloudComputeSoleTenancyNodeGroupsSetIamPolicyOptions(
[property: PositionalArgument] string NodeGroup,
[property: PositionalArgument] string Zone,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;