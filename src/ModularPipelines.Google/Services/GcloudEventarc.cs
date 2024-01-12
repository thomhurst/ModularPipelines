using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudEventarc
{
    public GcloudEventarc(
        GcloudEventarcAuditLogsProvider auditLogsProvider,
        GcloudEventarcChannelConnections channelConnections,
        GcloudEventarcChannels channels,
        GcloudEventarcGoogleChannels googleChannels,
        GcloudEventarcLocations locations,
        GcloudEventarcProviders providers,
        GcloudEventarcTriggers triggers
    )
    {
        AuditLogsProvider = auditLogsProvider;
        ChannelConnections = channelConnections;
        Channels = channels;
        GoogleChannels = googleChannels;
        Locations = locations;
        Providers = providers;
        Triggers = triggers;
    }

    public GcloudEventarcAuditLogsProvider AuditLogsProvider { get; }

    public GcloudEventarcChannelConnections ChannelConnections { get; }

    public GcloudEventarcChannels Channels { get; }

    public GcloudEventarcGoogleChannels GoogleChannels { get; }

    public GcloudEventarcLocations Locations { get; }

    public GcloudEventarcProviders Providers { get; }

    public GcloudEventarcTriggers Triggers { get; }
}