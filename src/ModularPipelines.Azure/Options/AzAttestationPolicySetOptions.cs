using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("attestation", "policy", "set")]
public record AzAttestationPolicySetOptions(
[property: CommandSwitch("--attestation-type")] string AttestationType
) : AzOptions
{
    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--new-attestation-policy")]
    public string? NewAttestationPolicy { get; set; }

    [CommandSwitch("--new-attestation-policy-file")]
    public string? NewAttestationPolicyFile { get; set; }

    [CommandSwitch("--policy-format")]
    public string? PolicyFormat { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}

