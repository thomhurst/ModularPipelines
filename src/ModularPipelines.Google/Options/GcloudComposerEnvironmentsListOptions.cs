using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("composer", "environments", "list")]
public record GcloudComposerEnvironmentsListOptions : GcloudOptions
{
    [CliOption("--locations")]
    public string[]? Locations { get; set; }
}