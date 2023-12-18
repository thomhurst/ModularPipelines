using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "perimeter", "association", "create")]
public record AzNetworkPerimeterAssociationCreateOptions(
[property: CommandSwitch("--association-name")] string AssociationName,
[property: CommandSwitch("--perimeter-name")] string PerimeterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--access-mode")]
    public string? AccessMode { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--private-link-resource")]
    public string? PrivateLinkResource { get; set; }

    [CommandSwitch("--profile")]
    public string? Profile { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}