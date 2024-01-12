using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "scopes", "get-iam-policy")]
public record GcloudContainerHubScopesGetIamPolicyOptions(
[property: PositionalArgument] string Scope
) : GcloudOptions;