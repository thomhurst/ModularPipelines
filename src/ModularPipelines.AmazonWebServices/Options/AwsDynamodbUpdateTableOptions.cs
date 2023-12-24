using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynamodb", "update-table")]
public record AwsDynamodbUpdateTableOptions(
[property: CommandSwitch("--table-name")] string TableName
) : AwsOptions
{
    [CommandSwitch("--attribute-definitions")]
    public string[]? AttributeDefinitions { get; set; }

    [CommandSwitch("--billing-mode")]
    public string? BillingMode { get; set; }

    [CommandSwitch("--provisioned-throughput")]
    public string? ProvisionedThroughput { get; set; }

    [CommandSwitch("--global-secondary-index-updates")]
    public string[]? GlobalSecondaryIndexUpdates { get; set; }

    [CommandSwitch("--stream-specification")]
    public string? StreamSpecification { get; set; }

    [CommandSwitch("--sse-specification")]
    public string? SseSpecification { get; set; }

    [CommandSwitch("--replica-updates")]
    public string[]? ReplicaUpdates { get; set; }

    [CommandSwitch("--table-class")]
    public string? TableClass { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}