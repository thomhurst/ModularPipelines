using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managedservices", "assignment", "show")]
public record AzManagedservicesAssignmentShowOptions(
[property: CommandSwitch("--assignment")] string Assignment
) : AzOptions
{
    [BooleanCommandSwitch("--include-definition")]
    public bool? IncludeDefinition { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}