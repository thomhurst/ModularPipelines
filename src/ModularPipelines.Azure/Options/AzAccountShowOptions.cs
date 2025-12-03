using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("account", "show")]
public record AzAccountShowOptions : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }
}