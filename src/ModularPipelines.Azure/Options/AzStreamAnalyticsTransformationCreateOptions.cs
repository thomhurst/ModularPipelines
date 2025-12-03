using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stream-analytics", "transformation", "create")]
public record AzStreamAnalyticsTransformationCreateOptions(
[property: CliOption("--job-name")] string JobName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CliOption("--saql")]
    public string? Saql { get; set; }

    [CliOption("--streaming-units")]
    public string? StreamingUnits { get; set; }

    [CliOption("--valid-streaming-units")]
    public string? ValidStreamingUnits { get; set; }
}