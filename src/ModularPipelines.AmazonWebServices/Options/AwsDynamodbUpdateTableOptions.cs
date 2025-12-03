using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodb", "update-table")]
public record AwsDynamodbUpdateTableOptions(
[property: CliOption("--table-name")] string TableName
) : AwsOptions
{
    [CliOption("--attribute-definitions")]
    public string[]? AttributeDefinitions { get; set; }

    [CliOption("--billing-mode")]
    public string? BillingMode { get; set; }

    [CliOption("--provisioned-throughput")]
    public string? ProvisionedThroughput { get; set; }

    [CliOption("--global-secondary-index-updates")]
    public string[]? GlobalSecondaryIndexUpdates { get; set; }

    [CliOption("--stream-specification")]
    public string? StreamSpecification { get; set; }

    [CliOption("--sse-specification")]
    public string? SseSpecification { get; set; }

    [CliOption("--replica-updates")]
    public string[]? ReplicaUpdates { get; set; }

    [CliOption("--table-class")]
    public string? TableClass { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}