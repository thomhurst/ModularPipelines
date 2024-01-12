using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("service-directory", "services", "remove-iam-policy-binding")]
public record GcloudServiceDirectoryServicesRemoveIamPolicyBindingOptions(
[property: PositionalArgument] string Service,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Namespace,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions;