using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("asset", "feeds", "describe")]
public record GcloudAssetFeedsDescribeOptions : GcloudOptions
{
    public GcloudAssetFeedsDescribeOptions(
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
}
