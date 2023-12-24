using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynamodbstreams", "list-streams")]
public record AwsDynamodbstreamsListStreamsOptions : AwsOptions
{
    [CommandSwitch("--table-name")]
    public string? TableName { get; set; }

    [CommandSwitch("--limit")]
    public int? Limit { get; set; }

    [CommandSwitch("--exclusive-start-stream-arn")]
    public string? ExclusiveStartStreamArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}