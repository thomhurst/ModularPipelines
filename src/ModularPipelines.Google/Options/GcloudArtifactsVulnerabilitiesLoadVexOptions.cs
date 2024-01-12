using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "vulnerabilities", "load-vex")]
public record GcloudArtifactsVulnerabilitiesLoadVexOptions(
[property: CommandSwitch("--source")] string Source,
[property: CommandSwitch("--uri")] string Uri
) : GcloudOptions
{
    [CommandSwitch("--project")]
    public new string? Project { get; set; }
}