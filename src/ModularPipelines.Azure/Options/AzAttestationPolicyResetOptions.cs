using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("attestation", "policy", "reset")]
public record AzAttestationPolicyResetOptions(
[property: CliOption("--attestation-type")] string AttestationType
) : AzOptions
{
    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--policy-jws")]
    public string? PolicyJws { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}