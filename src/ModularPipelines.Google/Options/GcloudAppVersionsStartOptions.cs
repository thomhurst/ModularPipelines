using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "versions", "start")]
public record GcloudAppVersionsStartOptions(
[property: CliArgument] string Versions
) : GcloudOptions
{
    [CliOption("--service")]
    public string? Service { get; set; }
}