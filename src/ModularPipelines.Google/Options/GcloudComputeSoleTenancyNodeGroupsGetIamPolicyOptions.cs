using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "sole-tenancy", "node-groups", "get-iam-policy")]
public record GcloudComputeSoleTenancyNodeGroupsGetIamPolicyOptions(
[property: CliArgument] string NodeGroup,
[property: CliArgument] string Zone
) : GcloudOptions;