using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudPubsub
{
    public GcloudPubsub(
        GcloudPubsubLiteOperations liteOperations,
        GcloudPubsubLiteReservations liteReservations,
        GcloudPubsubLiteSubscriptions liteSubscriptions,
        GcloudPubsubLiteTopics liteTopics,
        GcloudPubsubSchemas schemas,
        GcloudPubsubSnapshots snapshots,
        GcloudPubsubSubscriptions subscriptions,
        GcloudPubsubTopics topics
    )
    {
        LiteOperations = liteOperations;
        LiteReservations = liteReservations;
        LiteSubscriptions = liteSubscriptions;
        LiteTopics = liteTopics;
        Schemas = schemas;
        Snapshots = snapshots;
        Subscriptions = subscriptions;
        Topics = topics;
    }

    public GcloudPubsubLiteOperations LiteOperations { get; }

    public GcloudPubsubLiteReservations LiteReservations { get; }

    public GcloudPubsubLiteSubscriptions LiteSubscriptions { get; }

    public GcloudPubsubLiteTopics LiteTopics { get; }

    public GcloudPubsubSchemas Schemas { get; }

    public GcloudPubsubSnapshots Snapshots { get; }

    public GcloudPubsubSubscriptions Subscriptions { get; }

    public GcloudPubsubTopics Topics { get; }
}