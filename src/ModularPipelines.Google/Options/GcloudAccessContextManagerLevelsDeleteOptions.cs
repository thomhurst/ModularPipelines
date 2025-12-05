using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "levels", "delete")]
public record GcloudAccessContextManagerLevelsDeleteOptions(
[property: CliArgument] string Level,
[property: CliArgument] string Policy
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}