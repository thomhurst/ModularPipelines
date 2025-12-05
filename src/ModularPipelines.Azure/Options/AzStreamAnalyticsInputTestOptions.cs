using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("stream-analytics", "input", "test")]
public record AzStreamAnalyticsInputTestOptions(
[property: CliOption("--input-name")] string InputName,
[property: CliOption("--job-name")] string JobName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--properties")]
    public string? Properties { get; set; }
}