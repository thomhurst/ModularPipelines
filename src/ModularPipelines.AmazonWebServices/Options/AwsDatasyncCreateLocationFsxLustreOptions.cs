using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datasync", "create-location-fsx-lustre")]
public record AwsDatasyncCreateLocationFsxLustreOptions(
[property: CliOption("--fsx-filesystem-arn")] string FsxFilesystemArn,
[property: CliOption("--security-group-arns")] string[] SecurityGroupArns
) : AwsOptions
{
    [CliOption("--subdirectory")]
    public string? Subdirectory { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}