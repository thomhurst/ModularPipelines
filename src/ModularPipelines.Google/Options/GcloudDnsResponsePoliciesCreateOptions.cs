using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns", "response-policies", "create")]
public record GcloudDnsResponsePoliciesCreateOptions(
[property: PositionalArgument] string ResponsePolicies,
[property: CommandSwitch("--description")] string Description
) : GcloudOptions
{
    [CommandSwitch("--gkeclusters")]
    public string[]? Gkeclusters { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--networks")]
    public string[]? Networks { get; set; }
}