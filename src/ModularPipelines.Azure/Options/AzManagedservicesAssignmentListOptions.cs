using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managedservices", "assignment", "list")]
public record AzManagedservicesAssignmentListOptions(
[property: CommandSwitch("--assignment")] string Assignment
) : AzOptions
{
    [BooleanCommandSwitch("--include-definition")]
    public bool? IncludeDefinition { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}