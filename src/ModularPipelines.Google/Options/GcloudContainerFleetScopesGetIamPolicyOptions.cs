using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "scopes", "get-iam-policy")]
public record GcloudContainerFleetScopesGetIamPolicyOptions(
[property: PositionalArgument] string Scope
) : GcloudOptions;