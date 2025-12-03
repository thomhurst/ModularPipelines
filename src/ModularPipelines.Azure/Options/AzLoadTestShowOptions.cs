using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("load", "test", "show")]
public record AzLoadTestShowOptions(
[property: CliOption("--load-test-resource")] string LoadTestResource,
[property: CliOption("--test-id")] string TestId
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}