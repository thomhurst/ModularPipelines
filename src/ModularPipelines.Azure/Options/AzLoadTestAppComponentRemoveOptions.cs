using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("load", "test", "app-component", "remove")]
public record AzLoadTestAppComponentRemoveOptions(
[property: CliOption("--app-component-id")] string AppComponentId,
[property: CliOption("--load-test-resource")] string LoadTestResource,
[property: CliOption("--test-id")] string TestId
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}