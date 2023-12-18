using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hdinsight-on-aks", "cluster", "trino-hive-catalog", "create")]
public record AzHdinsightOnAksClusterTrinoHiveCatalogCreateOptions(
[property: CommandSwitch("--catalog-name")] string CatalogName,
[property: CommandSwitch("--metastore-db-connection-password-secret")] string MetastoreDbConnectionPasswordSecret,
[property: CommandSwitch("--metastore-db-connection-url")] string MetastoreDbConnectionUrl,
[property: CommandSwitch("--metastore-db-connection-user-name")] string MetastoreDbConnectionUserName
) : AzOptions
{
    [CommandSwitch("--metastore-warehouse-dir")]
    public string? MetastoreWarehouseDir { get; set; }
}