using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodb", "update-global-table")]
public record AwsDynamodbUpdateGlobalTableOptions(
[property: CliOption("--global-table-name")] string GlobalTableName,
[property: CliOption("--replica-updates")] string[] ReplicaUpdates
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}