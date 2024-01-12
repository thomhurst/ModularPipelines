using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns", "response-policies", "update")]
public record GcloudDnsResponsePoliciesUpdateOptions(
[property: PositionalArgument] string ResponsePolicies
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--gkeclusters")]
    public string[]? Gkeclusters { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--networks")]
    public string[]? Networks { get; set; }
}