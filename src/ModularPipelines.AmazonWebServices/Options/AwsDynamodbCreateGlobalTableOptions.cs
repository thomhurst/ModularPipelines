using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodb", "create-global-table")]
public record AwsDynamodbCreateGlobalTableOptions(
[property: CliOption("--global-table-name")] string GlobalTableName,
[property: CliOption("--replication-group")] string[] ReplicationGroup
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}