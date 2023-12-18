using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managedapp", "create")]
public record AzManagedappCreateOptions(
[property: CommandSwitch("--kind")] string Kind,
[property: CommandSwitch("--managed-rg-id")] string ManagedRgId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--managedapp-definition-id")]
    public string? ManagedappDefinitionId { get; set; }

    [CommandSwitch("--parameters")]
    public string? Parameters { get; set; }

    [CommandSwitch("--plan-name")]
    public string? PlanName { get; set; }

    [CommandSwitch("--plan-product")]
    public string? PlanProduct { get; set; }

    [CommandSwitch("--plan-publisher")]
    public string? PlanPublisher { get; set; }

    [CommandSwitch("--plan-version")]
    public string? PlanVersion { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

