using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datasync", "create-location-fsx-open-zfs")]
public record AwsDatasyncCreateLocationFsxOpenZfsOptions(
[property: CommandSwitch("--fsx-filesystem-arn")] string FsxFilesystemArn,
[property: CommandSwitch("--protocol")] string Protocol,
[property: CommandSwitch("--security-group-arns")] string[] SecurityGroupArns
) : AwsOptions
{
    [CommandSwitch("--subdirectory")]
    public string? Subdirectory { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}