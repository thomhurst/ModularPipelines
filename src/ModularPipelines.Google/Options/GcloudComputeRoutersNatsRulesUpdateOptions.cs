using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "routers", "nats", "rules", "update")]
public record GcloudComputeRoutersNatsRulesUpdateOptions(
[property: CliArgument] string RuleNumber,
[property: CliOption("--nat")] string Nat,
[property: CliOption("--router")] string Router
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--match")]
    public string? Match { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--source-nat-active-ips")]
    public string[]? SourceNatActiveIps { get; set; }

    [CliOption("--source-nat-active-ranges")]
    public string[]? SourceNatActiveRanges { get; set; }

    [CliFlag("--clear-source-nat-drain-ips")]
    public bool? ClearSourceNatDrainIps { get; set; }

    [CliOption("--source-nat-drain-ips")]
    public string[]? SourceNatDrainIps { get; set; }

    [CliFlag("--clear-source-nat-drain-ranges")]
    public bool? ClearSourceNatDrainRanges { get; set; }

    [CliOption("--source-nat-drain-ranges")]
    public string[]? SourceNatDrainRanges { get; set; }
}