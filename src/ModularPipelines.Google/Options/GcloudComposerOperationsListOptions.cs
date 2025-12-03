using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("composer", "operations", "list")]
public record GcloudComposerOperationsListOptions : GcloudOptions
{
    [CliOption("--locations")]
    public string[]? Locations { get; set; }
}