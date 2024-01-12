using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("asset", "analyze-move")]
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

    [CommandSwitch("--destination-folder")]
    public string DestinationFolder { get; set; }

    [CommandSwitch("--destination-organization")]
    public string DestinationOrganization { get; set; }

    [CommandSwitch("--blockers-only")]
    public string? BlockersOnly { get; set; }
}
