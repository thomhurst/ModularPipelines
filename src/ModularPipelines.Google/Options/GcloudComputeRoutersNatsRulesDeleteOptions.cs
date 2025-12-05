using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "routers", "nats", "rules", "delete")]
public record GcloudComputeRoutersNatsRulesDeleteOptions(
[property: CliArgument] string RuleNumber,
[property: CliOption("--nat")] string Nat,
[property: CliOption("--router")] string Router
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}