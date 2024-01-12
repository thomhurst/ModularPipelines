using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("asset", "get-history")]
public record GcloudAssetGetHistoryOptions : GcloudOptions
{
    public GcloudAssetGetHistoryOptions(
        string[] assetNames,
        string contentType,
        string startTime,
        string organization,
        string project
    )
    {
        AssetNames = assetNames;
        ContentType = contentType;
        StartTime = startTime;
        Organization = organization;
        Project = project;
    }

    [CommandSwitch("--asset-names")]
    public string[] AssetNames { get; set; }

    [CommandSwitch("--content-type")]
    public string ContentType { get; set; }

    [CommandSwitch("--start-time")]
    public string StartTime { get; set; }

    [CommandSwitch("--organization")]
    public string Organization { get; set; }

    [CommandSwitch("--end-time")]
    public string? EndTime { get; set; }

    [CommandSwitch("--relationship-types")]
    public string[]? RelationshipTypes { get; set; }
}
