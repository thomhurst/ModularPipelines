using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "cancel")]
public record GcloudBuildsCancelOptions(
[property: CliArgument] string Builds
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}