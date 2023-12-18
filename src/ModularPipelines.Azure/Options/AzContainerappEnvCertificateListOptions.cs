using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "env", "certificate", "list")]
public record AzContainerappEnvCertificateListOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--certificate")]
    public string? Certificate { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--managed-certificates-only")]
    public bool? ManagedCertificatesOnly { get; set; }

    [BooleanCommandSwitch("--private-key-certificates-only")]
    public bool? PrivateKeyCertificatesOnly { get; set; }

    [CommandSwitch("--thumbprint")]
    public string? Thumbprint { get; set; }
}

