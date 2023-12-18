using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "clone")]
public record AzMonitorCloneOptions(
[property: CommandSwitch("--source-resource")] string SourceResource,
[property: CommandSwitch("--target-resource")] string TargetResource
) : AzOptions
{
    [BooleanCommandSwitch("--always-clone")]
    public bool? AlwaysClone { get; set; }

    [CommandSwitch("--types")]
    public string? Types { get; set; }
}