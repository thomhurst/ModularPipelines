using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "sole-tenancy", "node-groups", "add-iam-policy-binding")]
public record GcloudComputeSoleTenancyNodeGroupsAddIamPolicyBindingOptions(
[property: PositionalArgument] string NodeGroup,
[property: PositionalArgument] string Zone,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions;