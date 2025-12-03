using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("attestation", "policy", "set")]
public record AzAttestationPolicySetOptions(
[property: CliOption("--attestation-type")] string AttestationType
) : AzOptions
{
    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--new-attestation-policy")]
    public string? NewAttestationPolicy { get; set; }

    [CliOption("--new-attestation-policy-file")]
    public string? NewAttestationPolicyFile { get; set; }

    [CliOption("--policy-format")]
    public string? PolicyFormat { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}