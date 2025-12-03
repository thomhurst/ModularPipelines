using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("load", "test-run", "stop")]
public record AzLoadTestRunStopOptions(
[property: CliOption("--load-test-resource")] string LoadTestResource,
[property: CliOption("--test-run-id")] string TestRunId
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}