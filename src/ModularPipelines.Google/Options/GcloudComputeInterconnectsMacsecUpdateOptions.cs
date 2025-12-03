using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "interconnects", "macsec", "update")]
public record GcloudComputeInterconnectsMacsecUpdateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliFlag("--enabled")]
    public bool? Enabled { get; set; }

    [CliFlag("--fail-open")]
    public bool? FailOpen { get; set; }
}