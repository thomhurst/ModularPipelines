using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "scopes", "get-iam-policy")]
public record GcloudContainerHubScopesGetIamPolicyOptions(
[property: CliArgument] string Scope
) : GcloudOptions;