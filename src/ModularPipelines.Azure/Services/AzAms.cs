using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzAms
{
    public AzAms(
        AzAmsAccount account,
        AzAmsAccountFilter accountFilter,
        AzAmsAsset asset,
        AzAmsAssetFilter assetFilter,
        AzAmsAssetTrack assetTrack,
        AzAmsContentKeyPolicy contentKeyPolicy,
        AzAmsJob job,
        AzAmsLiveEvent liveEvent,
        AzAmsLiveOutput liveOutput,
        AzAmsStreamingEndpoint streamingEndpoint,
        AzAmsStreamingLocator streamingLocator,
        AzAmsStreamingPolicy streamingPolicy,
        AzAmsTransform transform
    )
    {
        Account = account;
        AccountFilter = accountFilter;
        Asset = asset;
        AssetFilter = assetFilter;
        AssetTrack = assetTrack;
        ContentKeyPolicy = contentKeyPolicy;
        Job = job;
        LiveEvent = liveEvent;
        LiveOutput = liveOutput;
        StreamingEndpoint = streamingEndpoint;
        StreamingLocator = streamingLocator;
        StreamingPolicy = streamingPolicy;
        Transform = transform;
    }

    public AzAmsAccount Account { get; }

    public AzAmsAccountFilter AccountFilter { get; }

    public AzAmsAsset Asset { get; }

    public AzAmsAssetFilter AssetFilter { get; }

    public AzAmsAssetTrack AssetTrack { get; }

    public AzAmsContentKeyPolicy ContentKeyPolicy { get; }

    public AzAmsJob Job { get; }

    public AzAmsLiveEvent LiveEvent { get; }

    public AzAmsLiveOutput LiveOutput { get; }

    public AzAmsStreamingEndpoint StreamingEndpoint { get; }

    public AzAmsStreamingLocator StreamingLocator { get; }

    public AzAmsStreamingPolicy StreamingPolicy { get; }

    public AzAmsTransform Transform { get; }
}

