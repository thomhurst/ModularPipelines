using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "copy-snapshot")]
public record AwsEc2CopySnapshotOptions(
[property: CommandSwitch("--source-region")] string SourceRegion,
[property: CommandSwitch("--source-snapshot-id")] string SourceSnapshotId
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--destination-outpost-arn")]
    public string? DestinationOutpostArn { get; set; }

    [CommandSwitch("--destination-region")]
    public string? DestinationRegion { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--presigned-url")]
    public string? PresignedUrl { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}