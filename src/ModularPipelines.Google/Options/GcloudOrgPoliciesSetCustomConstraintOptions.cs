using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("org-policies", "set-custom-constraint")]
public record GcloudOrgPoliciesSetCustomConstraintOptions(
[property: PositionalArgument] string CustomConstraintFile
) : GcloudOptions;