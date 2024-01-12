using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("asset", "saved-queries", "delete")]
public record GcloudAssetSavedQueriesDeleteOptions : GcloudOptions
{
    public GcloudAssetSavedQueriesDeleteOptions(
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
}
