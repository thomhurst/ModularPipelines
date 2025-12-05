using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("policy", "attestation", "show")]
public record AzPolicyAttestationShowOptions(
[property: CliOption("--attestation-name")] string AttestationName
) : AzOptions
{
    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--parent")]
    public string? Parent { get; set; }

    [CliOption("--resource")]
    public string? Resource { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }
}