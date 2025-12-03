using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("account", "list")]
public record AzAccountListOptions : AzOptions
{
    [CliFlag("--all")]
    public bool? All { get; set; }

    [CliFlag("--refresh")]
    public bool? Refresh { get; set; }
}