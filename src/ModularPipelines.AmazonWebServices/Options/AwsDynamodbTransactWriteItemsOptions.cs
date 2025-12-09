using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodb", "transact-write-items")]
public record AwsDynamodbTransactWriteItemsOptions(
[property: CliOption("--transact-items")] string[] TransactItems
) : AwsOptions
{
    [CliOption("--return-consumed-capacity")]
    public string? ReturnConsumedCapacity { get; set; }

    [CliOption("--return-item-collection-metrics")]
    public string? ReturnItemCollectionMetrics { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}