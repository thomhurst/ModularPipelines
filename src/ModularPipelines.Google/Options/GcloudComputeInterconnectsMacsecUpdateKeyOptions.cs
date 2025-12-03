using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "interconnects", "macsec", "update-key")]
public record GcloudComputeInterconnectsMacsecUpdateKeyOptions(
[property: CliArgument] string Name,
[property: CliOption("--key-name")] string KeyName
) : GcloudOptions
{
    [CliOption("--start-time")]
    public string? StartTime { get; set; }
}