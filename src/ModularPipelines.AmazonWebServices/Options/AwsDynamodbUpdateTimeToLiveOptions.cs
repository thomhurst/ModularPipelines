using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodb", "update-time-to-live")]
public record AwsDynamodbUpdateTimeToLiveOptions(
[property: CliOption("--table-name")] string TableName,
[property: CliOption("--time-to-live-specification")] string TimeToLiveSpecification
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}