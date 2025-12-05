using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("stream-analytics", "input", "delete")]
public record AzStreamAnalyticsInputDeleteOptions(
[property: CliOption("--input-name")] string InputName,
[property: CliOption("--job-name")] string JobName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}