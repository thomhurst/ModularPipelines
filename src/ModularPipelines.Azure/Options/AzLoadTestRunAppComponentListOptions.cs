using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("load", "test-run", "app-component", "list")]
public record AzLoadTestRunAppComponentListOptions(
[property: CliOption("--load-test-resource")] string LoadTestResource,
[property: CliOption("--test-run-id")] string TestRunId
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}