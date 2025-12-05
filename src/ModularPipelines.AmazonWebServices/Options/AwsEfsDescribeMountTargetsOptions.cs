using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("efs", "describe-mount-targets")]
public record AwsEfsDescribeMountTargetsOptions : AwsOptions
{
    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--file-system-id")]
    public string? FileSystemId { get; set; }

    [CliOption("--mount-target-id")]
    public string? MountTargetId { get; set; }

    [CliOption("--access-point-id")]
    public string? AccessPointId { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}