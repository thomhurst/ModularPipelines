using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redis", "operations", "list")]
public record GcloudRedisOperationsListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}