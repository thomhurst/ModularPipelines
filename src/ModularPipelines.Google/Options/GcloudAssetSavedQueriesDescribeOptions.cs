using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("asset", "saved-queries", "describe")]
public record GcloudAssetSavedQueriesDescribeOptions : GcloudOptions
{
    public GcloudAssetSavedQueriesDescribeOptions(
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
}
