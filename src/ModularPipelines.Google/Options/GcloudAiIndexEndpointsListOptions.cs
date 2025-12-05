using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "index-endpoints", "list")]
public record GcloudAiIndexEndpointsListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}