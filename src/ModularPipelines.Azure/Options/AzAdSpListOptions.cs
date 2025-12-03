using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ad", "sp", "list")]
public record AzAdSpListOptions : AzOptions
{
    [CliOption("--all")]
    public string? All { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--show-mine")]
    public string? ShowMine { get; set; }

    [CliOption("--spn")]
    public string? Spn { get; set; }
}