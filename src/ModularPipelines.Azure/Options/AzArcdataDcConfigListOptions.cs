using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("arcdata", "dc", "config", "list")]
public record AzArcdataDcConfigListOptions : AzOptions
{
    [CliOption("--config-profile")]
    public string? ConfigProfile { get; set; }
}