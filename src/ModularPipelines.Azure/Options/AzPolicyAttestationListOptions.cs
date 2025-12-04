using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("policy", "attestation", "list")]
public record AzPolicyAttestationListOptions : AzOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--resource")]
    public string? Resource { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}