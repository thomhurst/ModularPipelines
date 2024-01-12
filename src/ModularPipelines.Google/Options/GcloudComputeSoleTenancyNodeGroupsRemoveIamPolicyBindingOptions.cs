using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "sole-tenancy", "node-groups", "remove-iam-policy-binding")]
public record GcloudComputeSoleTenancyNodeGroupsRemoveIamPolicyBindingOptions(
[property: PositionalArgument] string NodeGroup,
[property: PositionalArgument] string Zone,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions;