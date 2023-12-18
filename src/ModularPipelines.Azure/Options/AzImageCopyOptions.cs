using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("image", "copy")]
public record AzImageCopyOptions(
[property: CommandSwitch("--source-object-name")] string SourceObjectName,
[property: CommandSwitch("--source-resource-group")] string SourceResourceGroup,
[property: CommandSwitch("--target-location")] string TargetLocation,
[property: CommandSwitch("--target-resource-group")] string TargetResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--cleanup")]
    public bool? Cleanup { get; set; }

    [BooleanCommandSwitch("--export-as-snapshot")]
    public bool? ExportAsSnapshot { get; set; }

    [CommandSwitch("--parallel-degree")]
    public string? ParallelDegree { get; set; }

    [CommandSwitch("--source-type")]
    public string? SourceType { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--target-name")]
    public string? TargetName { get; set; }

    [CommandSwitch("--target-subscription")]
    public string? TargetSubscription { get; set; }

    [CommandSwitch("--temporary-resource-group-name")]
    public string? TemporaryResourceGroupName { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }
}

