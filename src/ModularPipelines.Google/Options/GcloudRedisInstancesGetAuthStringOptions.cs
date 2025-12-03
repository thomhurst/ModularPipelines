using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redis", "instances", "get-auth-string")]
public record GcloudRedisInstancesGetAuthStringOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Region
) : GcloudOptions;