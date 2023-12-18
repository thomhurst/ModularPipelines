using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "app", "permission", "grant")]
public record AzAdAppPermissionGrantOptions(
[property: CommandSwitch("--api")] string Api,
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--scope")] string Scope
) : AzOptions
{
    [CommandSwitch("--consent-type")]
    public string? ConsentType { get; set; }

    [CommandSwitch("--principal-id")]
    public string? PrincipalId { get; set; }
}