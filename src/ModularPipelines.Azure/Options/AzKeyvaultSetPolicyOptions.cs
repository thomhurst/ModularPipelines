using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "set-policy")]
public record AzKeyvaultSetPolicyOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--application-id")]
    public string? ApplicationId { get; set; }

    [CommandSwitch("--certificate-permissions")]
    public string? CertificatePermissions { get; set; }

    [CommandSwitch("--key-permissions")]
    public string? KeyPermissions { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--object-id")]
    public string? ObjectId { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--secret-permissions")]
    public string? SecretPermissions { get; set; }

    [CommandSwitch("--spn")]
    public string? Spn { get; set; }

    [CommandSwitch("--storage-permissions")]
    public string? StoragePermissions { get; set; }

    [CommandSwitch("--upn")]
    public string? Upn { get; set; }
}

