using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stream-analytics", "function", "test")]
public record AzStreamAnalyticsFunctionTestOptions(
[property: CliOption("--function-name")] string FunctionName,
[property: CliOption("--job-name")] string JobName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--properties")]
    public string? Properties { get; set; }
}