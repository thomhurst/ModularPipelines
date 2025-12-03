using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("asset", "feeds", "update")]
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

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string FeedId { get; set; }

    [CliOption("--folder")]
    public string Folder { get; set; }

    [CliOption("--organization")]
    public string Organization { get; set; }

    [CliOption("--pubsub-topic")]
    public string? PubsubTopic { get; set; }

    [CliOption("--add-asset-names")]
    public string[]? AddAssetNames { get; set; }

    [CliFlag("--clear-asset-names")]
    public bool? ClearAssetNames { get; set; }

    [CliOption("--remove-asset-names")]
    public string[]? RemoveAssetNames { get; set; }

    [CliOption("--add-asset-types")]
    public string[]? AddAssetTypes { get; set; }

    [CliFlag("--clear-asset-types")]
    public bool? ClearAssetTypes { get; set; }

    [CliOption("--remove-asset-types")]
    public string[]? RemoveAssetTypes { get; set; }

    [CliOption("--add-relationship-types")]
    public string[]? AddRelationshipTypes { get; set; }

    [CliFlag("--clear-relationship-types")]
    public bool? ClearRelationshipTypes { get; set; }

    [CliOption("--remove-relationship-types")]
    public string[]? RemoveRelationshipTypes { get; set; }

    [CliFlag("--clear-condition-description")]
    public bool? ClearConditionDescription { get; set; }

    [CliOption("--condition-description")]
    public string? ConditionDescription { get; set; }

    [CliFlag("--clear-condition-expression")]
    public bool? ClearConditionExpression { get; set; }

    [CliOption("--condition-expression")]
    public string? ConditionExpression { get; set; }

    [CliFlag("--clear-condition-title")]
    public bool? ClearConditionTitle { get; set; }

    [CliOption("--condition-title")]
    public string? ConditionTitle { get; set; }

    [CliFlag("--clear-content-type")]
    public bool? ClearContentType { get; set; }

    [CliOption("--content-type")]
    public string? ContentType { get; set; }
}
