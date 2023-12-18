using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzServicebus
{
    public AzServicebus(
        AzServicebusGeorecoveryAlias georecoveryAlias,
        AzServicebusMigration migration,
        AzServicebusNamespace @namespace,
        AzServicebusQueue queue,
        AzServicebusTopic topic
    )
    {
        GeorecoveryAlias = georecoveryAlias;
        Migration = migration;
        Namespace = @namespace;
        Queue = queue;
        Topic = topic;
    }

    public AzServicebusGeorecoveryAlias GeorecoveryAlias { get; }

    public AzServicebusMigration Migration { get; }

    public AzServicebusNamespace Namespace { get; }

    public AzServicebusQueue Queue { get; }

    public AzServicebusTopic Topic { get; }
}