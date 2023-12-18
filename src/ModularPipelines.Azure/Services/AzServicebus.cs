using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzServicebus
{
    public AzServicebus(
        AzServicebusGeorecoveryAlias georecoveryAlias,
        AzServicebusMigration migration,
        AzServicebusNamespace @Namespace,
        AzServicebusQueue queue,
        AzServicebusTopic topic
    )
    {
        GeorecoveryAlias = georecoveryAlias;
        Migration = migration;
        Namespace = @Namespace;
        Queue = queue;
        Topic = topic;
    }

    public AzServicebusGeorecoveryAlias GeorecoveryAlias { get; }

    public AzServicebusMigration Migration { get; }

    public AzServicebusNamespace Namespace { get; }

    public AzServicebusQueue Queue { get; }

    public AzServicebusTopic Topic { get; }
}