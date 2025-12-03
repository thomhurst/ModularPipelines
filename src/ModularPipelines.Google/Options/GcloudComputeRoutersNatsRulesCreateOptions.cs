using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "routers", "nats", "rules", "create")]
public record GcloudComputeRoutersNatsRulesCreateOptions(
[property: CliArgument] string RuleNumber,
[property: CliOption("--match")] string Match,
[property: CliOption("--nat")] string Nat,
[property: CliOption("--router")] string Router
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--source-nat-active-ips")]
    public string[]? SourceNatActiveIps { get; set; }

    [CliOption("--source-nat-active-ranges")]
    public string[]? SourceNatActiveRanges { get; set; }
}