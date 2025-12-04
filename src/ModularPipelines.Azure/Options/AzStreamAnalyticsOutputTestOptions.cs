using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("stream-analytics", "output", "test")]
public record AzStreamAnalyticsOutputTestOptions(
[property: CliOption("--job-name")] string JobName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--datasource")]
    public string? Datasource { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--serialization")]
    public string? Serialization { get; set; }

    [CliOption("--size-window")]
    public string? SizeWindow { get; set; }

    [CliOption("--time-window")]
    public string? TimeWindow { get; set; }
}