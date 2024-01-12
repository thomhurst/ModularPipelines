using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("asset", "feeds", "create")]
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

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string FeedId { get; set; }

    [CommandSwitch("--pubsub-topic")]
    public string PubsubTopic { get; set; }

    [CommandSwitch("--asset-names")]
    public string[] AssetNames { get; set; }

    [CommandSwitch("--asset-types")]
    public string[] AssetTypes { get; set; }

    [CommandSwitch("--relationship-types")]
    public string[] RelationshipTypes { get; set; }

    [CommandSwitch("--folder")]
    public string Folder { get; set; }

    [CommandSwitch("--organization")]
    public string Organization { get; set; }

    [CommandSwitch("--condition-description")]
    public string? ConditionDescription { get; set; }

    [CommandSwitch("--condition-expression")]
    public string? ConditionExpression { get; set; }

    [CommandSwitch("--condition-title")]
    public string? ConditionTitle { get; set; }

    [CommandSwitch("--content-type")]
    public string? ContentType { get; set; }
}
