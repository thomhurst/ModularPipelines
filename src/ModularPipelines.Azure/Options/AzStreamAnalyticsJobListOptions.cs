using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stream-analytics", "job", "list")]
public record AzStreamAnalyticsJobListOptions : AzOptions
{
    [CliOption("--expand")]
    public string? Expand { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}