using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("asset", "feeds", "update")]
public record GcloudAssetFeedsUpdateOptions : GcloudOptions
{
    public GcloudAssetFeedsUpdateOptions(
        string feedId,
        string folder,
        string organization,
        string project
    )
    {
        FeedId = feedId;
        Folder = folder;
        Organization = organization;
        Project = project;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string FeedId { get; set; }

    [CommandSwitch("--folder")]
    public string Folder { get; set; }

    [CommandSwitch("--organization")]
    public string Organization { get; set; }

    [CommandSwitch("--pubsub-topic")]
    public string? PubsubTopic { get; set; }

    [CommandSwitch("--add-asset-names")]
    public string[]? AddAssetNames { get; set; }

    [BooleanCommandSwitch("--clear-asset-names")]
    public bool? ClearAssetNames { get; set; }

    [CommandSwitch("--remove-asset-names")]
    public string[]? RemoveAssetNames { get; set; }

    [CommandSwitch("--add-asset-types")]
    public string[]? AddAssetTypes { get; set; }

    [BooleanCommandSwitch("--clear-asset-types")]
    public bool? ClearAssetTypes { get; set; }

    [CommandSwitch("--remove-asset-types")]
    public string[]? RemoveAssetTypes { get; set; }

    [CommandSwitch("--add-relationship-types")]
    public string[]? AddRelationshipTypes { get; set; }

    [BooleanCommandSwitch("--clear-relationship-types")]
    public bool? ClearRelationshipTypes { get; set; }

    [CommandSwitch("--remove-relationship-types")]
    public string[]? RemoveRelationshipTypes { get; set; }

    [BooleanCommandSwitch("--clear-condition-description")]
    public bool? ClearConditionDescription { get; set; }

    [CommandSwitch("--condition-description")]
    public string? ConditionDescription { get; set; }

    [BooleanCommandSwitch("--clear-condition-expression")]
    public bool? ClearConditionExpression { get; set; }

    [CommandSwitch("--condition-expression")]
    public string? ConditionExpression { get; set; }

    [BooleanCommandSwitch("--clear-condition-title")]
    public bool? ClearConditionTitle { get; set; }

    [CommandSwitch("--condition-title")]
    public string? ConditionTitle { get; set; }

    [BooleanCommandSwitch("--clear-content-type")]
    public bool? ClearContentType { get; set; }

    [CommandSwitch("--content-type")]
    public string? ContentType { get; set; }
}
