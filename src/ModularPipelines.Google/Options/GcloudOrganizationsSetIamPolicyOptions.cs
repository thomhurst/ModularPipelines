using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("organizations", "set-iam-policy")]
public record GcloudOrganizationsSetIamPolicyOptions(
[property: CliArgument] string OrganizationId,
[property: CliArgument] string PolicyFile
) : GcloudOptions;