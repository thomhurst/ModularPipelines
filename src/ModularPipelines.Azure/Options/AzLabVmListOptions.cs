using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lab", "vm", "list")]
public record AzLabVmListOptions(
[property: CommandSwitch("--lab-name")] string LabName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--all")]
    public string? All { get; set; }

    [CommandSwitch("--claimable")]
    public string? Claimable { get; set; }

    [CommandSwitch("--environment")]
    public string? Environment { get; set; }

    [CommandSwitch("--expand")]
    public string? Expand { get; set; }

    [CommandSwitch("--filters")]
    public string? Filters { get; set; }

    [CommandSwitch("--object-id")]
    public string? ObjectId { get; set; }

    [CommandSwitch("--order-by")]
    public string? OrderBy { get; set; }

    [CommandSwitch("--top")]
    public string? Top { get; set; }
}

