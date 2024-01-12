using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "vulnerabilities", "list")]
public record GcloudArtifactsVulnerabilitiesListOptions(
[property: PositionalArgument] string Uri
) : GcloudOptions
{
    [CommandSwitch("--occurrence-filter")]
    public string? OccurrenceFilter { get; set; }
}