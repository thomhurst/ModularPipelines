using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("image", "copy")]
public record AzImageCopyOptions(
[property: CliOption("--source-object-name")] string SourceObjectName,
[property: CliOption("--source-resource-group")] string SourceResourceGroup,
[property: CliOption("--target-location")] string TargetLocation,
[property: CliOption("--target-resource-group")] string TargetResourceGroup
) : AzOptions
{
    [CliFlag("--cleanup")]
    public bool? Cleanup { get; set; }

    [CliFlag("--export-as-snapshot")]
    public bool? ExportAsSnapshot { get; set; }

    [CliOption("--parallel-degree")]
    public string? ParallelDegree { get; set; }

    [CliOption("--source-type")]
    public string? SourceType { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--target-name")]
    public string? TargetName { get; set; }

    [CliOption("--target-subscription")]
    public string? TargetSubscription { get; set; }

    [CliOption("--temporary-resource-group-name")]
    public string? TemporaryResourceGroupName { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}