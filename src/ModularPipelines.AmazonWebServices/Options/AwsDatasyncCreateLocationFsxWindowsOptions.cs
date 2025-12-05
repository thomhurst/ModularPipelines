using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datasync", "create-location-fsx-windows")]
public record AwsDatasyncCreateLocationFsxWindowsOptions(
[property: CliOption("--fsx-filesystem-arn")] string FsxFilesystemArn,
[property: CliOption("--security-group-arns")] string[] SecurityGroupArns,
[property: CliOption("--user")] string User,
[property: CliOption("--password")] string Password
) : AwsOptions
{
    [CliOption("--subdirectory")]
    public string? Subdirectory { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--domain")]
    public string? Domain { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}