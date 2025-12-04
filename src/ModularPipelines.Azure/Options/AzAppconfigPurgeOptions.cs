using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("appconfig", "purge")]
public record AzAppconfigPurgeOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}