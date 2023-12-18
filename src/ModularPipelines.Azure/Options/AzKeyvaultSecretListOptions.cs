using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "secret", "list")]
public record AzKeyvaultSecretListOptions(
[property: CommandSwitch("--file")] string File,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [BooleanCommandSwitch("--include-managed")]
    public bool? IncludeManaged { get; set; }

    [CommandSwitch("--maxresults")]
    public string? Maxresults { get; set; }
}

