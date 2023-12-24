using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynamodb", "list-global-tables")]
public record AwsDynamodbListGlobalTablesOptions : AwsOptions
{
    [CommandSwitch("--exclusive-start-global-table-name")]
    public string? ExclusiveStartGlobalTableName { get; set; }

    [CommandSwitch("--limit")]
    public int? Limit { get; set; }

    [CommandSwitch("--region-name")]
    public string? RegionName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}