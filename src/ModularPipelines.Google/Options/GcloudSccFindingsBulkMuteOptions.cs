using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scc", "findings", "bulk-mute")]
public record GcloudSccFindingsBulkMuteOptions : GcloudOptions
{
    public GcloudSccFindingsBulkMuteOptions(
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

    [CliOption("--filter")]
    public string? Filter { get; set; }
}
