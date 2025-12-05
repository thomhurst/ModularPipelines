using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "response-policies", "create")]
public record GcloudDnsResponsePoliciesCreateOptions(
[property: CliArgument] string ResponsePolicies,
[property: CliOption("--description")] string Description
) : GcloudOptions
{
    [CliOption("--gkeclusters")]
    public string[]? Gkeclusters { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--networks")]
    public string[]? Networks { get; set; }
}