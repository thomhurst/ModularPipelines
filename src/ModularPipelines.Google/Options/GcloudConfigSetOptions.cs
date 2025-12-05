using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("config", "set")]
public record GcloudConfigSetOptions(
[property: CliArgument] string Section,
[property: CliArgument] string Value
) : GcloudOptions
{
    [CliFlag("--installation")]
    public bool? Installation { get; set; }
}