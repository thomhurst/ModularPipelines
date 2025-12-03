using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "response-policies", "update")]
public record GcloudDnsResponsePoliciesUpdateOptions(
[property: CliArgument] string ResponsePolicies
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--gkeclusters")]
    public string[]? Gkeclusters { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--networks")]
    public string[]? Networks { get; set; }
}