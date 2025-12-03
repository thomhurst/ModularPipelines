using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodbstreams", "list-streams")]
public record AwsDynamodbstreamsListStreamsOptions : AwsOptions
{
    [CliOption("--table-name")]
    public string? TableName { get; set; }

    [CliOption("--limit")]
    public int? Limit { get; set; }

    [CliOption("--exclusive-start-stream-arn")]
    public string? ExclusiveStartStreamArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}