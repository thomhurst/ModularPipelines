using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "product", "test", "task", "show")]
public record AzIotProductTestTaskShowOptions(
[property: CommandSwitch("--test-id")] string TestId
) : AzOptions
{
    [CommandSwitch("--base-url")]
    public string? BaseUrl { get; set; }

    [BooleanCommandSwitch("--running")]
    public bool? Running { get; set; }

    [CommandSwitch("--task-id")]
    public string? TaskId { get; set; }
}