using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "scopes", "remove-iam-policy-binding")]
public record GcloudContainerHubScopesRemoveIamPolicyBindingOptions(
[property: PositionalArgument] string Scope,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions;