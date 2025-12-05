using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("org-policies", "describe-custom-constraint")]
public record GcloudOrgPoliciesDescribeCustomConstraintOptions(
[property: CliArgument] string CustomConstraint,
[property: CliOption("--organization")] string Organization
) : GcloudOptions;