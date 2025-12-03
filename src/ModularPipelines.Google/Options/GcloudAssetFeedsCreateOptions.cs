using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("asset", "feeds", "create")]
public record GcloudAssetFeedsCreateOptions : GcloudOptions
{
    public GcloudAssetFeedsCreateOptions(
        string feedId,
        string pubsubTopic,
        string[] assetNames,
        string[] assetTypes,
        string[] relationshipTypes,
        string folder,
        string organization,
        string project
    )
    {
        FeedId = feedId;
        PubsubTopic = pubsubTopic;
        AssetNames = assetNames;
        AssetTypes = assetTypes;
        RelationshipTypes = relationshipTypes;
        Folder = folder;
        Organization = organization;
        Project = project;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string FeedId { get; set; }

    [CliOption("--pubsub-topic")]
    public string PubsubTopic { get; set; }

    [CliOption("--asset-names")]
    public string[] AssetNames { get; set; }

    [CliOption("--asset-types")]
    public string[] AssetTypes { get; set; }

    [CliOption("--relationship-types")]
    public string[] RelationshipTypes { get; set; }

    [CliOption("--folder")]
    public string Folder { get; set; }

    [CliOption("--organization")]
    public string Organization { get; set; }

    [CliOption("--condition-description")]
    public string? ConditionDescription { get; set; }

    [CliOption("--condition-expression")]
    public string? ConditionExpression { get; set; }

    [CliOption("--condition-title")]
    public string? ConditionTitle { get; set; }

    [CliOption("--content-type")]
    public string? ContentType { get; set; }
}
