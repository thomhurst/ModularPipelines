using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("asset", "saved-queries", "create")]
public record GcloudAssetSavedQueriesCreateOptions : GcloudOptions
{
    public GcloudAssetSavedQueriesCreateOptions(
        string queryId,
        string queryFilePath,
        string folder,
        string organization,
        string project
    )
    {
        QueryId = queryId;
        QueryFilePath = queryFilePath;
        Folder = folder;
        Organization = organization;
        Project = project;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string QueryId { get; set; }

    [CliOption("--query-file-path")]
    public string QueryFilePath { get; set; }

    [CliOption("--folder")]
    public string Folder { get; set; }

    [CliOption("--organization")]
    public string Organization { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}
