using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "response-policies", "describe")]
public record GcloudDnsResponsePoliciesDescribeOptions(
[property: CliArgument] string ResponsePolicies
) : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}