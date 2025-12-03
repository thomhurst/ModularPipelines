using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("orbital", "spacecraft", "contact", "show")]
public record AzOrbitalSpacecraftContactShowOptions : AzOptions
{
    [CliOption("--contact-name")]
    public string? ContactName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--spacecraft-name")]
    public string? SpacecraftName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}