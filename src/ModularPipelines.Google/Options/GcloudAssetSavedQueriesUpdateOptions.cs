using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("asset", "saved-queries", "update")]
public record GcloudAssetSavedQueriesUpdateOptions : GcloudOptions
{
    public GcloudAssetSavedQueriesUpdateOptions(
        string queryId,
        string folder,
        string organization,
        string project
    )
    {
        QueryId = queryId;
        Folder = folder;
        Organization = organization;
        Project = project;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string QueryId { get; set; }

    [CliOption("--folder")]
    public string Folder { get; set; }

    [CliOption("--organization")]
    public string Organization { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--query-file-path")]
    public string? QueryFilePath { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }
}
