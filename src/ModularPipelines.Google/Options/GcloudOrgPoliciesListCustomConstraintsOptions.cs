using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("org-policies", "list-custom-constraints")]
public record GcloudOrgPoliciesListCustomConstraintsOptions(
[property: CliOption("--organization")] string Organization
) : GcloudOptions;