using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "scopes", "get-iam-policy")]
public record GcloudContainerFleetScopesGetIamPolicyOptions(
[property: CliArgument] string Scope
) : GcloudOptions;