using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fzf", "location")]
public record AzFzfLocationOptions : AzOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliFlag("--no-default")]
    public bool? NoDefault { get; set; }
}