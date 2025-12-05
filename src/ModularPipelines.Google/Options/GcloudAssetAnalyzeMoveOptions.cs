using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("asset", "analyze-move")]
public record GcloudAssetAnalyzeMoveOptions : GcloudOptions
{
    public GcloudAssetAnalyzeMoveOptions(
        string project,
        string destinationFolder,
        string destinationOrganization
    )
    {
        Project = project;
        DestinationFolder = destinationFolder;
        DestinationOrganization = destinationOrganization;
    }

    [CliOption("--destination-folder")]
    public string DestinationFolder { get; set; }

    [CliOption("--destination-organization")]
    public string DestinationOrganization { get; set; }

    [CliOption("--blockers-only")]
    public string? BlockersOnly { get; set; }
}
