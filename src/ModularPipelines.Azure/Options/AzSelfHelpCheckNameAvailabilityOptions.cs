using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("self-help", "check-name-availability")]
public record AzSelfHelpCheckNameAvailabilityOptions(
[property: CliOption("--scope")] string Scope
) : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }
}