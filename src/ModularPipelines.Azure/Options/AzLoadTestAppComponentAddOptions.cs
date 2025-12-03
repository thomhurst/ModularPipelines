using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("load", "test", "app-component", "add")]
public record AzLoadTestAppComponentAddOptions(
[property: CliOption("--app-component-id")] string AppComponentId,
[property: CliOption("--app-component-name")] string AppComponentName,
[property: CliOption("--app-component-type")] string AppComponentType,
[property: CliOption("--load-test-resource")] string LoadTestResource,
[property: CliOption("--test-id")] string TestId
) : AzOptions
{
    [CliOption("--app-component-kind")]
    public string? AppComponentKind { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}