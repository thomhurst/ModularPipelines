using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("load", "test-run", "app-component", "add")]
public record AzLoadTestRunAppComponentAddOptions(
[property: CommandSwitch("--app-component-id")] string AppComponentId,
[property: CommandSwitch("--app-component-name")] string AppComponentName,
[property: CommandSwitch("--app-component-type")] string AppComponentType,
[property: CommandSwitch("--load-test-resource")] string LoadTestResource,
[property: CommandSwitch("--test-run-id")] string TestRunId
) : AzOptions
{
    [CommandSwitch("--app-component-kind")]
    public string? AppComponentKind { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}