using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datasync", "create-location-fsx-windows")]
public record AwsDatasyncCreateLocationFsxWindowsOptions(
[property: CommandSwitch("--fsx-filesystem-arn")] string FsxFilesystemArn,
[property: CommandSwitch("--security-group-arns")] string[] SecurityGroupArns,
[property: CommandSwitch("--user")] string User,
[property: CommandSwitch("--password")] string Password
) : AwsOptions
{
    [CommandSwitch("--subdirectory")]
    public string? Subdirectory { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--domain")]
    public string? Domain { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}