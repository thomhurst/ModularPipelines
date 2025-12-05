using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "copy-image")]
public record AwsEc2CopyImageOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--source-image-id")] string SourceImageId,
[property: CliOption("--source-region")] string SourceRegion
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--destination-outpost-arn")]
    public string? DestinationOutpostArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}