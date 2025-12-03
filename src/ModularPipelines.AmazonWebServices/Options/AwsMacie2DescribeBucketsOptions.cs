using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("macie2", "describe-buckets")]
public record AwsMacie2DescribeBucketsOptions : AwsOptions
{
    [CliOption("--criteria")]
    public IEnumerable<KeyValue>? Criteria { get; set; }

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