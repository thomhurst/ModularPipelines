using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stream-analytics", "output", "create")]
public record AzStreamAnalyticsOutputCreateOptions(
[property: CliOption("--job-name")] string JobName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--datasource")]
    public string? Datasource { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CliOption("--serialization")]
    public string? Serialization { get; set; }

    [CliOption("--size-window")]
    public string? SizeWindow { get; set; }

    [CliOption("--time-window")]
    public string? TimeWindow { get; set; }
}