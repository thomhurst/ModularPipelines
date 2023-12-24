using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datasync", "create-location-efs")]
public record AwsDatasyncCreateLocationEfsOptions(
[property: CommandSwitch("--efs-filesystem-arn")] string EfsFilesystemArn,
[property: CommandSwitch("--ec2-config")] string Ec2Config
) : AwsOptions
{
    [CommandSwitch("--subdirectory")]
    public string? Subdirectory { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--access-point-arn")]
    public string? AccessPointArn { get; set; }

    [CommandSwitch("--file-system-access-role-arn")]
    public string? FileSystemAccessRoleArn { get; set; }

    [CommandSwitch("--in-transit-encryption")]
    public string? InTransitEncryption { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}