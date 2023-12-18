using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "certificate", "list")]
public record AzKeyvaultCertificateListOptions : AzOptions
{
    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [BooleanCommandSwitch("--include-pending")]
    public bool? IncludePending { get; set; }

    [CommandSwitch("--maxresults")]
    public string? Maxresults { get; set; }

    [CommandSwitch("--vault-name")]
    public string? VaultName { get; set; }
}