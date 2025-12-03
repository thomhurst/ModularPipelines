using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "endpoints", "list")]
public record GcloudAiEndpointsListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}