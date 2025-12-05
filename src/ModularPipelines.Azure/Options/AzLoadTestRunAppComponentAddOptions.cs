using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("load", "test-run", "app-component", "add")]
public record AzLoadTestRunAppComponentAddOptions(
[property: CliOption("--app-component-id")] string AppComponentId,
[property: CliOption("--app-component-name")] string AppComponentName,
[property: CliOption("--app-component-type")] string AppComponentType,
[property: CliOption("--load-test-resource")] string LoadTestResource,
[property: CliOption("--test-run-id")] string TestRunId
) : AzOptions
{
    [CliOption("--app-component-kind")]
    public string? AppComponentKind { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}