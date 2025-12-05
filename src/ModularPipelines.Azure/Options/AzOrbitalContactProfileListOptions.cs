using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("orbital", "contact-profile", "list")]
public record AzOrbitalContactProfileListOptions : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--skiptoken")]
    public string? Skiptoken { get; set; }
}