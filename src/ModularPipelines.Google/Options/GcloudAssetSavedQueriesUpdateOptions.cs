using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("asset", "saved-queries", "update")]
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

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string QueryId { get; set; }

    [CommandSwitch("--folder")]
    public string Folder { get; set; }

    [CommandSwitch("--organization")]
    public string Organization { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--query-file-path")]
    public string? QueryFilePath { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }
}
