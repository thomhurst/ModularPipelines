using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "connections", "add-iam-policy-binding")]
public record GcloudBuildsConnectionsAddIamPolicyBindingOptions(
[property: CliArgument] string Connection,
[property: CliArgument] string Region,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;