using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "secret", "list-versions")]
public record AzKeyvaultSecretListVersionsOptions : AzOptions
{
    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [CommandSwitch("--maxresults")]
    public string? Maxresults { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--vault-name")]
    public string? VaultName { get; set; }
}