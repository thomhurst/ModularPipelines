using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "product", "test", "case", "list")]
public record AzIotProductTestCaseListOptions(
[property: CommandSwitch("--test-id")] string TestId
) : AzOptions
{
    [CommandSwitch("--base-url")]
    public string? BaseUrl { get; set; }
}

