using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "product", "requirement", "list")]
public record AzIotProductRequirementListOptions : AzOptions
{
    [CommandSwitch("--badge-type")]
    public string? BadgeType { get; set; }

    [CommandSwitch("--base-url")]
    public string? BaseUrl { get; set; }
}