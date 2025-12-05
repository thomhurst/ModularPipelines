using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "scopes", "add-iam-policy-binding")]
public record GcloudContainerHubScopesAddIamPolicyBindingOptions(
[property: CliArgument] string Scope,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;