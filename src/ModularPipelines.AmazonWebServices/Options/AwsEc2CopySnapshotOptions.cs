using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "copy-snapshot")]
public record AwsEc2CopySnapshotOptions(
[property: CliOption("--source-region")] string SourceRegion,
[property: CliOption("--source-snapshot-id")] string SourceSnapshotId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--destination-outpost-arn")]
    public string? DestinationOutpostArn { get; set; }

    [CliOption("--destination-region")]
    public string? DestinationRegion { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--presigned-url")]
    public string? PresignedUrl { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}