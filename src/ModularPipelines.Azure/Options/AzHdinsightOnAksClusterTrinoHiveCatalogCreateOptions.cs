using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("hdinsight-on-aks", "cluster", "trino-hive-catalog", "create")]
public record AzHdinsightOnAksClusterTrinoHiveCatalogCreateOptions(
[property: CliOption("--catalog-name")] string CatalogName,
[property: CliOption("--metastore-db-connection-password-secret")] string MetastoreDbConnectionPasswordSecret,
[property: CliOption("--metastore-db-connection-url")] string MetastoreDbConnectionUrl,
[property: CliOption("--metastore-db-connection-user-name")] string MetastoreDbConnectionUserName
) : AzOptions
{
    [CliOption("--metastore-warehouse-dir")]
    public string? MetastoreWarehouseDir { get; set; }
}