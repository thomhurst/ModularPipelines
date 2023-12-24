using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynamodb", "create-global-table")]
public record AwsDynamodbCreateGlobalTableOptions(
[property: CommandSwitch("--global-table-name")] string GlobalTableName,
[property: CommandSwitch("--replication-group")] string[] ReplicationGroup
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}