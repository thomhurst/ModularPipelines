using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("asset", "get-history")]
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

    [CliOption("--asset-names")]
    public string[] AssetNames { get; set; }

    [CliOption("--content-type")]
    public string ContentType { get; set; }

    [CliOption("--start-time")]
    public string StartTime { get; set; }

    [CliOption("--organization")]
    public string Organization { get; set; }

    [CliOption("--end-time")]
    public string? EndTime { get; set; }

    [CliOption("--relationship-types")]
    public string[]? RelationshipTypes { get; set; }
}
