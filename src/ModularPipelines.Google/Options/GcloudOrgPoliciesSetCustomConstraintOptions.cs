using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("org-policies", "set-custom-constraint")]
public record GcloudOrgPoliciesSetCustomConstraintOptions(
[property: CliArgument] string CustomConstraintFile
) : GcloudOptions;