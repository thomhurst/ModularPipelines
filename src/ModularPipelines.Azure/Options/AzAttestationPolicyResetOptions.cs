using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("attestation", "policy", "reset")]
public record AzAttestationPolicyResetOptions(
[property: CommandSwitch("--attestation-type")] string AttestationType
) : AzOptions
{
    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--policy-jws")]
    public string? PolicyJws { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}