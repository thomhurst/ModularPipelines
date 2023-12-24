using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynamodb", "create-table")]
public record AwsDynamodbCreateTableOptions(
[property: CommandSwitch("--attribute-definitions")] string[] AttributeDefinitions,
[property: CommandSwitch("--table-name")] string TableName,
[property: CommandSwitch("--key-schema")] string[] KeySchema
) : AwsOptions
{
    [CommandSwitch("--local-secondary-indexes")]
    public string[]? LocalSecondaryIndexes { get; set; }

    [CommandSwitch("--global-secondary-indexes")]
    public string[]? GlobalSecondaryIndexes { get; set; }

    [CommandSwitch("--billing-mode")]
    public string? BillingMode { get; set; }

    [CommandSwitch("--provisioned-throughput")]
    public string? ProvisionedThroughput { get; set; }

    [CommandSwitch("--stream-specification")]
    public string? StreamSpecification { get; set; }

    [CommandSwitch("--sse-specification")]
    public string? SseSpecification { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--table-class")]
    public string? TableClass { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}