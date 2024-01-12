using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("org-policies", "list-custom-constraints")]
public record GcloudOrgPoliciesListCustomConstraintsOptions(
[property: CommandSwitch("--organization")] string Organization
) : GcloudOptions;