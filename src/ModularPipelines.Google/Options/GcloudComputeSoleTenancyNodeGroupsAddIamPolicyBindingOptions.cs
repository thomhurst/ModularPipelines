using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "sole-tenancy", "node-groups", "add-iam-policy-binding")]
public record GcloudComputeSoleTenancyNodeGroupsAddIamPolicyBindingOptions(
[property: CliArgument] string NodeGroup,
[property: CliArgument] string Zone,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;