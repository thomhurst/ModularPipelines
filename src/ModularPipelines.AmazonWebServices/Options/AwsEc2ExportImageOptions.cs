using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "export-image")]
public record AwsEc2ExportImageOptions(
[property: CliOption("--disk-image-format")] string DiskImageFormat,
[property: CliOption("--image-id")] string ImageId,
[property: CliOption("--s3-export-location")] string S3ExportLocation
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--role-name")]
    public string? RoleName { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}