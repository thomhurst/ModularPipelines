using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("tag", "delete")]
public record AzTagDeleteOptions : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-id")]
    public string? ResourceId { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}