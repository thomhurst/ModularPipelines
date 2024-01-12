using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("organizations", "set-iam-policy")]
public record GcloudOrganizationsSetIamPolicyOptions(
[property: PositionalArgument] string OrganizationId,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;