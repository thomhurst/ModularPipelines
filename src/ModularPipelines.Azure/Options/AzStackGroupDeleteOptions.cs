using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stack", "group", "delete")]
public record AzStackGroupDeleteOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--delete-all")]
    public bool? DeleteAll { get; set; }

    [BooleanCommandSwitch("--delete-resource-groups")]
    public bool? DeleteResourceGroups { get; set; }

    [BooleanCommandSwitch("--delete-resources")]
    public bool? DeleteResources { get; set; }

    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}