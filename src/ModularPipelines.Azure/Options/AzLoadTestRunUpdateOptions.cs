using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("load", "test-run", "update")]
public record AzLoadTestRunUpdateOptions(
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--load-test-resource")] string LoadTestResource,
[property: CommandSwitch("--test-run-id")] string TestRunId
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}