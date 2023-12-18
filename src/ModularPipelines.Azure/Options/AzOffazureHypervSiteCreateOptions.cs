using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("offure", "hyperv", "site", "create")]
public record AzOffazureHypervSiteCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--agent")]
    public string? Agent { get; set; }

    [CommandSwitch("--appliance-name")]
    public string? ApplianceName { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--solution-id")]
    public string? SolutionId { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

