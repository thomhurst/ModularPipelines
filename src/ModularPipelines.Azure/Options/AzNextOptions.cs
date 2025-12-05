using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("next")]
public record AzNextOptions : AzOptions
{
    [CliFlag("--command")]
    public bool? Command { get; set; }

    [CliFlag("--scenario")]
    public bool? Scenario { get; set; }
}