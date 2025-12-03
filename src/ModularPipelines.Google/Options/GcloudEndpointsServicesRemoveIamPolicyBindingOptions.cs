using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("endpoints", "services", "remove-iam-policy-binding")]
public record GcloudEndpointsServicesRemoveIamPolicyBindingOptions(
[property: CliArgument] string Service,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;