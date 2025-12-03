using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redis", "operations", "cancel")]
public record GcloudRedisOperationsCancelOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Region
) : GcloudOptions;