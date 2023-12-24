using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("efs", "describe-mount-targets")]
public record AwsEfsDescribeMountTargetsOptions : AwsOptions
{
    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--file-system-id")]
    public string? FileSystemId { get; set; }

    [CommandSwitch("--mount-target-id")]
    public string? MountTargetId { get; set; }

    [CommandSwitch("--access-point-id")]
    public string? AccessPointId { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}