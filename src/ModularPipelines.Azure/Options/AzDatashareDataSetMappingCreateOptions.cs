using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datashare", "data-set-mapping", "create")]
public record AzDatashareDataSetMappingCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--data-set-mapping-name")] string DataSetMappingName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--share-subscription-name")] string ShareSubscriptionName
) : AzOptions
{
    [CliOption("--adls-gen2-file-data-set-mapping")]
    public string? AdlsGen2FileDataSetMapping { get; set; }

    [CliOption("--adls-gen2-file-system-data-set-mapping")]
    public string? AdlsGen2FileSystemDataSetMapping { get; set; }

    [CliOption("--adls-gen2-folder-data-set-mapping")]
    public string? AdlsGen2FolderDataSetMapping { get; set; }

    [CliOption("--blob-container-data-set-mapping")]
    public string? BlobContainerDataSetMapping { get; set; }

    [CliOption("--blob-data-set-mapping")]
    public string? BlobDataSetMapping { get; set; }

    [CliOption("--blob-folder-data-set-mapping")]
    public string? BlobFolderDataSetMapping { get; set; }

    [CliOption("--kusto-cluster-data-set-mapping")]
    public string? KustoClusterDataSetMapping { get; set; }

    [CliOption("--kusto-database-data-set-mapping")]
    public string? KustoDatabaseDataSetMapping { get; set; }

    [CliOption("--kusto-table-data-set-mapping")]
    public string? KustoTableDataSetMapping { get; set; }

    [CliOption("--sqldb-table-data-set-mapping")]
    public string? SqldbTableDataSetMapping { get; set; }

    [CliOption("--sqldw-table-data-set-mapping")]
    public string? SqldwTableDataSetMapping { get; set; }

    [CliOption("--synapse-workspace-sql-pool-table-data-set-mapping")]
    public string? SynapseWorkspaceSqlPoolTableDataSetMapping { get; set; }
}