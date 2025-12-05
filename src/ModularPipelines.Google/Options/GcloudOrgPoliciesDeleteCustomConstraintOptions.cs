using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("org-policies", "delete-custom-constraint")]
public record GcloudOrgPoliciesDeleteCustomConstraintOptions(
[property: CliArgument] string CustomConstraint,
[property: CliOption("--organization")] string Organization
) : GcloudOptions;