using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("asset", "feeds", "delete")]
public record GcloudAssetFeedsDeleteOptions : GcloudOptions
{
    public GcloudAssetFeedsDeleteOptions(
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
}
