using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("config", "unset")]
public record GcloudConfigUnsetOptions(
[property: CliArgument] string Section
) : GcloudOptions
{
    [CliFlag("--installation")]
    public bool? Installation { get; set; }
}