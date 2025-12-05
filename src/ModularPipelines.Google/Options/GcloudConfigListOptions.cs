using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("config", "list")]
public record GcloudConfigListOptions(
[property: CliArgument] string Section
) : GcloudOptions
{
    [CliFlag("--all")]
    public bool? All { get; set; }
}