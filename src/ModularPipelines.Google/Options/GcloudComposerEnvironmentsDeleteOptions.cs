using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("composer", "environments", "delete")]
public record GcloudComposerEnvironmentsDeleteOptions(
[property: CliArgument] string Environments,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}