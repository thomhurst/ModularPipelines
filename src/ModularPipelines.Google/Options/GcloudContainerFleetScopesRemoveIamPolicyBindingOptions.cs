using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "scopes", "remove-iam-policy-binding")]
public record GcloudContainerFleetScopesRemoveIamPolicyBindingOptions(
[property: CliArgument] string Scope,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;