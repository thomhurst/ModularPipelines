using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

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