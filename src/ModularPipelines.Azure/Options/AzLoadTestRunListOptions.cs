using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("load", "test-run", "list")]
public record AzLoadTestRunListOptions(
[property: CliOption("--load-test-resource")] string LoadTestResource,
[property: CliOption("--test-id")] string TestId
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}