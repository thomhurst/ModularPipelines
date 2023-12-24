using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynamodb", "transact-write-items")]
public record AwsDynamodbTransactWriteItemsOptions(
[property: CommandSwitch("--transact-items")] string[] TransactItems
) : AwsOptions
{
    [CommandSwitch("--return-consumed-capacity")]
    public string? ReturnConsumedCapacity { get; set; }

    [CommandSwitch("--return-item-collection-metrics")]
    public string? ReturnItemCollectionMetrics { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}