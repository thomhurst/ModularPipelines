using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "create-data-set")]
public record AwsQuicksightCreateDataSetOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--data-set-id")] string DataSetId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--physical-table-map")] IEnumerable<KeyValue> PhysicalTableMap,
[property: CommandSwitch("--import-mode")] string ImportMode
) : AwsOptions
{
    [CommandSwitch("--logical-table-map")]
    public IEnumerable<KeyValue>? LogicalTableMap { get; set; }

    [CommandSwitch("--column-groups")]
    public string[]? ColumnGroups { get; set; }

    [CommandSwitch("--field-folders")]
    public IEnumerable<KeyValue>? FieldFolders { get; set; }

    [CommandSwitch("--permissions")]
    public string[]? Permissions { get; set; }

    [CommandSwitch("--row-level-permission-data-set")]
    public string? RowLevelPermissionDataSet { get; set; }

    [CommandSwitch("--row-level-permission-tag-configuration")]
    public string? RowLevelPermissionTagConfiguration { get; set; }

    [CommandSwitch("--column-level-permission-rules")]
    public string[]? ColumnLevelPermissionRules { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--data-set-usage-configuration")]
    public string? DataSetUsageConfiguration { get; set; }

    [CommandSwitch("--dataset-parameters")]
    public string[]? DatasetParameters { get; set; }

    [CommandSwitch("--folder-arns")]
    public string[]? FolderArns { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}