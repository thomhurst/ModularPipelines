using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodb", "list-global-tables")]
public record AwsDynamodbListGlobalTablesOptions : AwsOptions
{
    [CliOption("--exclusive-start-global-table-name")]
    public string? ExclusiveStartGlobalTableName { get; set; }

    [CliOption("--limit")]
    public int? Limit { get; set; }

    [CliOption("--region-name")]
    public string? RegionName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}