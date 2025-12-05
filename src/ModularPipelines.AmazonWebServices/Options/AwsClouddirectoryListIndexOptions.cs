using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("clouddirectory", "list-index")]
public record AwsClouddirectoryListIndexOptions(
[property: CliOption("--directory-arn")] string DirectoryArn,
[property: CliOption("--index-reference")] string IndexReference
) : AwsOptions
{
    [CliOption("--ranges-on-indexed-values")]
    public string[]? RangesOnIndexedValues { get; set; }

    [CliOption("--consistency-level")]
    public string? ConsistencyLevel { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}