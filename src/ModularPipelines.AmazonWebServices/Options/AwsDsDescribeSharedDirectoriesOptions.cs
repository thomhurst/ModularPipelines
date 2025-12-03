using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "describe-shared-directories")]
public record AwsDsDescribeSharedDirectoriesOptions(
[property: CliOption("--owner-directory-id")] string OwnerDirectoryId
) : AwsOptions
{
    [CliOption("--shared-directory-ids")]
    public string[]? SharedDirectoryIds { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}