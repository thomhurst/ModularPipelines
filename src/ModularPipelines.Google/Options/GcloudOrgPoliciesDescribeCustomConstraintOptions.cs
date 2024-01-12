using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("org-policies", "describe-custom-constraint")]
public record GcloudOrgPoliciesDescribeCustomConstraintOptions(
[property: PositionalArgument] string CustomConstraint,
[property: CommandSwitch("--organization")] string Organization
) : GcloudOptions;