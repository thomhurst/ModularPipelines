using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "sole-tenancy", "node-groups", "remove-iam-policy-binding")]
public record GcloudComputeSoleTenancyNodeGroupsRemoveIamPolicyBindingOptions(
[property: CliArgument] string NodeGroup,
[property: CliArgument] string Zone,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;