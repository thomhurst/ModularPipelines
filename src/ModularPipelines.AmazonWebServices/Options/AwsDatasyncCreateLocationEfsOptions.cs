using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datasync", "create-location-efs")]
public record AwsDatasyncCreateLocationEfsOptions(
[property: CliOption("--efs-filesystem-arn")] string EfsFilesystemArn,
[property: CliOption("--ec2-config")] string Ec2Config
) : AwsOptions
{
    [CliOption("--subdirectory")]
    public string? Subdirectory { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--access-point-arn")]
    public string? AccessPointArn { get; set; }

    [CliOption("--file-system-access-role-arn")]
    public string? FileSystemAccessRoleArn { get; set; }

    [CliOption("--in-transit-encryption")]
    public string? InTransitEncryption { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}