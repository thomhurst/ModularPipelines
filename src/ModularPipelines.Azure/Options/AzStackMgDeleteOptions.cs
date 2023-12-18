using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stack", "mg", "delete")]
public record AzStackMgDeleteOptions(
[property: CommandSwitch("--management-group-id")] string ManagementGroupId
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