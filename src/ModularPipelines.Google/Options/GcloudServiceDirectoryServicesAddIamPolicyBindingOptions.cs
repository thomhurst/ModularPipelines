using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("service-directory", "services", "add-iam-policy-binding")]
public record GcloudServiceDirectoryServicesAddIamPolicyBindingOptions(
[property: PositionalArgument] string Service,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Namespace,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions;