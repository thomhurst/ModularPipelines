using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("organizations", "get-iam-policy")]
public record GcloudOrganizationsGetIamPolicyOptions(
[property: PositionalArgument] string OrganizationId
) : GcloudOptions;