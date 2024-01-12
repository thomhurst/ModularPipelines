using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "sbom", "list")]
public record GcloudArtifactsSbomListOptions : GcloudOptions
{
    [CommandSwitch("--dependency")]
    public string? Dependency { get; set; }

    [CommandSwitch("--resource")]
    public string? Resource { get; set; }

    [CommandSwitch("--resource-prefix")]
    public string? ResourcePrefix { get; set; }
}