using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connection", "create", "keyvault")]
public record AzConnectionCreateKeyvaultOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--client-type")]
    public string? ClientType { get; set; }

    [CommandSwitch("--connection")]
    public string? Connection { get; set; }

    [CommandSwitch("--customized-keys")]
    public string? CustomizedKeys { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--service-principal")]
    public string? ServicePrincipal { get; set; }

    [CommandSwitch("--target-id")]
    public string? TargetId { get; set; }

    [CommandSwitch("--target-resource-group")]
    public string? TargetResourceGroup { get; set; }

    [CommandSwitch("--user-account")]
    public int? UserAccount { get; set; }

    [CommandSwitch("--vault")]
    public string? Vault { get; set; }
}

