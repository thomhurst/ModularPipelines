using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("upgrade")]
public record AzUpgradeOptions : AzOptions
{
    [CliFlag("--all")]
    public bool? All { get; set; }

    [CliOption("--yes")]
    public bool? Yes { get; set; } = true;
}