using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "routers", "nats", "rules", "update")]
public record GcloudComputeRoutersNatsRulesUpdateOptions(
[property: PositionalArgument] string RuleNumber,
[property: CommandSwitch("--nat")] string Nat,
[property: CommandSwitch("--router")] string Router
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--match")]
    public string? Match { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--source-nat-active-ips")]
    public string[]? SourceNatActiveIps { get; set; }

    [CommandSwitch("--source-nat-active-ranges")]
    public string[]? SourceNatActiveRanges { get; set; }

    [BooleanCommandSwitch("--clear-source-nat-drain-ips")]
    public bool? ClearSourceNatDrainIps { get; set; }

    [CommandSwitch("--source-nat-drain-ips")]
    public string[]? SourceNatDrainIps { get; set; }

    [BooleanCommandSwitch("--clear-source-nat-drain-ranges")]
    public bool? ClearSourceNatDrainRanges { get; set; }

    [CommandSwitch("--source-nat-drain-ranges")]
    public string[]? SourceNatDrainRanges { get; set; }
}