using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ppg", "show")]
public record AzPpgShowOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--include-colocation-status")]
    public bool? IncludeColocationStatus { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}