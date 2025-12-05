using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "instances", "enable-debug")]
public record GcloudAppInstancesEnableDebugOptions(
[property: CliArgument] string Instance
) : GcloudOptions
{
    [CliOption("--service")]
    public string? Service { get; set; }

    [CliOption("--version")]
    public new string? Version { get; set; }
}