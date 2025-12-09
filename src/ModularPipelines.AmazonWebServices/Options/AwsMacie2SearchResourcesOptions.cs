using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("macie2", "search-resources")]
public record AwsMacie2SearchResourcesOptions : AwsOptions
{
    [CliOption("--bucket-criteria")]
    public string? BucketCriteria { get; set; }

    [CliOption("--sort-criteria")]
    public string? SortCriteria { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}