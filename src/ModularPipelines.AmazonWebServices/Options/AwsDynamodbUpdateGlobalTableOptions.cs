using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynamodb", "update-global-table")]
public record AwsDynamodbUpdateGlobalTableOptions(
[property: CommandSwitch("--global-table-name")] string GlobalTableName,
[property: CommandSwitch("--replica-updates")] string[] ReplicaUpdates
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}