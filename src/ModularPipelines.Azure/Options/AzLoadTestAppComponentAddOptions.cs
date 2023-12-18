using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("load", "test", "app-component", "add")]
public record AzLoadTestAppComponentAddOptions(
[property: CommandSwitch("--app-component-id")] string AppComponentId,
[property: CommandSwitch("--app-component-name")] string AppComponentName,
[property: CommandSwitch("--app-component-type")] string AppComponentType,
[property: CommandSwitch("--load-test-resource")] string LoadTestResource,
[property: CommandSwitch("--test-id")] string TestId
) : AzOptions
{
    [CommandSwitch("--app-component-kind")]
    public string? AppComponentKind { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}