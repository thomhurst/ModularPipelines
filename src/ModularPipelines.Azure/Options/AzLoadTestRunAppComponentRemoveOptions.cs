using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("load", "test-run", "app-component", "remove")]
public record AzLoadTestRunAppComponentRemoveOptions(
[property: CommandSwitch("--app-component-id")] string AppComponentId,
[property: CommandSwitch("--load-test-resource")] string LoadTestResource,
[property: CommandSwitch("--test-run-id")] string TestRunId
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}

