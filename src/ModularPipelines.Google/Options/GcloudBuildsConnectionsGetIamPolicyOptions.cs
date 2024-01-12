using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "connections", "get-iam-policy")]
public record GcloudBuildsConnectionsGetIamPolicyOptions(
[property: PositionalArgument] string Connection,
[property: PositionalArgument] string Region
) : GcloudOptions;