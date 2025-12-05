using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "update-data-set")]
public record AwsQuicksightUpdateDataSetOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--data-set-id")] string DataSetId,
[property: CliOption("--name")] string Name,
[property: CliOption("--physical-table-map")] IEnumerable<KeyValue> PhysicalTableMap,
[property: CliOption("--import-mode")] string ImportMode
) : AwsOptions
{
    [CliOption("--logical-table-map")]
    public IEnumerable<KeyValue>? LogicalTableMap { get; set; }

    [CliOption("--column-groups")]
    public string[]? ColumnGroups { get; set; }

    [CliOption("--field-folders")]
    public IEnumerable<KeyValue>? FieldFolders { get; set; }

    [CliOption("--row-level-permission-data-set")]
    public string? RowLevelPermissionDataSet { get; set; }

    [CliOption("--row-level-permission-tag-configuration")]
    public string? RowLevelPermissionTagConfiguration { get; set; }

    [CliOption("--column-level-permission-rules")]
    public string[]? ColumnLevelPermissionRules { get; set; }

    [CliOption("--data-set-usage-configuration")]
    public string? DataSetUsageConfiguration { get; set; }

    [CliOption("--dataset-parameters")]
    public string[]? DatasetParameters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}