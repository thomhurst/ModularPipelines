using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "token", "create")]
public record AzAcrTokenCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--registry")] string Registry
) : AzOptions
{
    [CommandSwitch("--expiration")]
    public string? Expiration { get; set; }

    [CommandSwitch("--expiration-in-days")]
    public string? ExpirationInDays { get; set; }

    [CommandSwitch("--gateway")]
    public string? Gateway { get; set; }

    [BooleanCommandSwitch("--no-passwords")]
    public bool? NoPasswords { get; set; }

    [CommandSwitch("--repository")]
    public string? Repository { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--scope-map")]
    public string? ScopeMap { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }
}