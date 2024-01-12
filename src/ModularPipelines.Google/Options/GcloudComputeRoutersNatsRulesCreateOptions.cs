using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "routers", "nats", "rules", "create")]
public record GcloudComputeRoutersNatsRulesCreateOptions(
[property: PositionalArgument] string RuleNumber,
[property: CommandSwitch("--match")] string Match,
[property: CommandSwitch("--nat")] string Nat,
[property: CommandSwitch("--router")] string Router
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--source-nat-active-ips")]
    public string[]? SourceNatActiveIps { get; set; }

    [CommandSwitch("--source-nat-active-ranges")]
    public string[]? SourceNatActiveRanges { get; set; }
}