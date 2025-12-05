using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("endpoints", "services", "add-iam-policy-binding")]
public record GcloudEndpointsServicesAddIamPolicyBindingOptions(
[property: CliArgument] string Service,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;