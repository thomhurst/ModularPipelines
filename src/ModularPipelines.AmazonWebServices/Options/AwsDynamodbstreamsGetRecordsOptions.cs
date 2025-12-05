using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodbstreams", "get-records")]
public record AwsDynamodbstreamsGetRecordsOptions(
[property: CliOption("--shard-iterator")] string ShardIterator
) : AwsOptions
{
    [CliOption("--limit")]
    public int? Limit { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}