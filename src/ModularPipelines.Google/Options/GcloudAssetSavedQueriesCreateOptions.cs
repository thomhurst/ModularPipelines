using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("asset", "saved-queries", "create")]
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

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string QueryId { get; set; }

    [CommandSwitch("--query-file-path")]
    public string QueryFilePath { get; set; }

    [CommandSwitch("--folder")]
    public string Folder { get; set; }

    [CommandSwitch("--organization")]
    public string Organization { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}
