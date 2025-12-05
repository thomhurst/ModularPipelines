using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodb", "create-table")]
public record AwsDynamodbCreateTableOptions(
[property: CliOption("--attribute-definitions")] string[] AttributeDefinitions,
[property: CliOption("--table-name")] string TableName,
[property: CliOption("--key-schema")] string[] KeySchema
) : AwsOptions
{
    [CliOption("--local-secondary-indexes")]
    public string[]? LocalSecondaryIndexes { get; set; }

    [CliOption("--global-secondary-indexes")]
    public string[]? GlobalSecondaryIndexes { get; set; }

    [CliOption("--billing-mode")]
    public string? BillingMode { get; set; }

    [CliOption("--provisioned-throughput")]
    public string? ProvisionedThroughput { get; set; }

    [CliOption("--stream-specification")]
    public string? StreamSpecification { get; set; }

    [CliOption("--sse-specification")]
    public string? SseSpecification { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--table-class")]
    public string? TableClass { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}