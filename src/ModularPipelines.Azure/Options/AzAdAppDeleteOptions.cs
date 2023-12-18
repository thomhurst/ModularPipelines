using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "app", "delete")]
public record AzAdAppDeleteOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions
{
    [CommandSwitch("--all")]
    public string? All { get; set; }

    [CommandSwitch("--app-id")]
    public string? AppId { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--identifier-uri")]
    public string? IdentifierUri { get; set; }

    [CommandSwitch("--show-mine")]
    public string? ShowMine { get; set; }
}