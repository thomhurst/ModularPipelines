using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "secret", "list")]
public record AzKeyvaultSecretListOptions : AzOptions
{
    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [BooleanCommandSwitch("--include-managed")]
    public bool? IncludeManaged { get; set; }

    [CommandSwitch("--maxresults")]
    public string? Maxresults { get; set; }

    [CommandSwitch("--vault-name")]
    public string? VaultName { get; set; }
}