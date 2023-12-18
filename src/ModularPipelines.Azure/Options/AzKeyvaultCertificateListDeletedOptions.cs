using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "certificate", "list-deleted")]
public record AzKeyvaultCertificateListDeletedOptions(
[property: CommandSwitch("--file")] string File
) : AzOptions
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