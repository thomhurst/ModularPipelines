using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "sole-tenancy", "node-groups", "set-iam-policy")]
public record GcloudComputeSoleTenancyNodeGroupsSetIamPolicyOptions(
[property: CliArgument] string NodeGroup,
[property: CliArgument] string Zone,
[property: CliArgument] string PolicyFile
) : GcloudOptions;