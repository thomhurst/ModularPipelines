using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "response-policies", "list")]
public record GcloudDnsResponsePoliciesListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}