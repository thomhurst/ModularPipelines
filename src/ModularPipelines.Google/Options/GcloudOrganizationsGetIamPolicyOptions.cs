using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("organizations", "get-iam-policy")]
public record GcloudOrganizationsGetIamPolicyOptions(
[property: CliArgument] string OrganizationId
) : GcloudOptions;