using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("asset", "list")]
public record GcloudAssetListOptions : GcloudOptions
{
    public GcloudAssetListOptions(
        string folder,
        string organization,
        string project
    )
    {
        Folder = folder;
        Organization = organization;
        Project = project;
    }

    [CliOption("--folder")]
    public string Folder { get; set; }

    [CliOption("--organization")]
    public string Organization { get; set; }

    [CliOption("--asset-types")]
    public string[]? AssetTypes { get; set; }

    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliOption("--relationship-types")]
    public string[]? RelationshipTypes { get; set; }

    [CliOption("--snapshot-time")]
    public string? SnapshotTime { get; set; }
}
