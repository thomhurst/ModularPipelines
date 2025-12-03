using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "connections", "get-iam-policy")]
public record GcloudBuildsConnectionsGetIamPolicyOptions(
[property: CliArgument] string Connection,
[property: CliArgument] string Region
) : GcloudOptions;