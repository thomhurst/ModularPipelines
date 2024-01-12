using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("org-policies", "delete-custom-constraint")]
public record GcloudOrgPoliciesDeleteCustomConstraintOptions(
[property: PositionalArgument] string CustomConstraint,
[property: CommandSwitch("--organization")] string Organization
) : GcloudOptions;