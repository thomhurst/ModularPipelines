using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("endpoints", "services", "add-iam-policy-binding")]
public record GcloudEndpointsServicesAddIamPolicyBindingOptions(
[property: PositionalArgument] string Service,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions;