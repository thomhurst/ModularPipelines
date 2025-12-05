using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "describe-update-directory")]
public record AwsDsDescribeUpdateDirectoryOptions(
[property: CliOption("--directory-id")] string DirectoryId,
[property: CliOption("--update-type")] string UpdateType
) : AwsOptions
{
    [CliOption("--region-name")]
    public string? RegionName { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}