using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("service-directory", "namespaces", "add-iam-policy-binding")]
public record GcloudServiceDirectoryNamespacesAddIamPolicyBindingOptions(
[property: PositionalArgument] string Namespace,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions;