using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("asset", "list")]
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

    [CommandSwitch("--folder")]
    public string Folder { get; set; }

    [CommandSwitch("--organization")]
    public string Organization { get; set; }

    [CommandSwitch("--asset-types")]
    public string[]? AssetTypes { get; set; }

    [CommandSwitch("--content-type")]
    public string? ContentType { get; set; }

    [CommandSwitch("--relationship-types")]
    public string[]? RelationshipTypes { get; set; }

    [CommandSwitch("--snapshot-time")]
    public string? SnapshotTime { get; set; }
}
