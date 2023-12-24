using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "export-image")]
public record AwsEc2ExportImageOptions(
[property: CommandSwitch("--disk-image-format")] string DiskImageFormat,
[property: CommandSwitch("--image-id")] string ImageId,
[property: CommandSwitch("--s3-export-location")] string S3ExportLocation
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--role-name")]
    public string? RoleName { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}