using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datashare", "data-set-mapping", "create")]
public record AzDatashareDataSetMappingCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--data-set-mapping-name")] string DataSetMappingName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--share-subscription-name")] string ShareSubscriptionName
) : AzOptions
{
    [CommandSwitch("--adls-gen2-file-data-set-mapping")]
    public string? AdlsGen2FileDataSetMapping { get; set; }

    [CommandSwitch("--adls-gen2-file-system-data-set-mapping")]
    public string? AdlsGen2FileSystemDataSetMapping { get; set; }

    [CommandSwitch("--adls-gen2-folder-data-set-mapping")]
    public string? AdlsGen2FolderDataSetMapping { get; set; }

    [CommandSwitch("--blob-container-data-set-mapping")]
    public string? BlobContainerDataSetMapping { get; set; }

    [CommandSwitch("--blob-data-set-mapping")]
    public string? BlobDataSetMapping { get; set; }

    [CommandSwitch("--blob-folder-data-set-mapping")]
    public string? BlobFolderDataSetMapping { get; set; }

    [CommandSwitch("--kusto-cluster-data-set-mapping")]
    public string? KustoClusterDataSetMapping { get; set; }

    [CommandSwitch("--kusto-database-data-set-mapping")]
    public string? KustoDatabaseDataSetMapping { get; set; }

    [CommandSwitch("--kusto-table-data-set-mapping")]
    public string? KustoTableDataSetMapping { get; set; }

    [CommandSwitch("--sqldb-table-data-set-mapping")]
    public string? SqldbTableDataSetMapping { get; set; }

    [CommandSwitch("--sqldw-table-data-set-mapping")]
    public string? SqldwTableDataSetMapping { get; set; }

    [CommandSwitch("--synapse-workspace-sql-pool-table-data-set-mapping")]
    public string? SynapseWorkspaceSqlPoolTableDataSetMapping { get; set; }
}