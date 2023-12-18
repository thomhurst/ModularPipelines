using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managedservices", "assignment", "delete")]
public record AzManagedservicesAssignmentDeleteOptions(
[property: CommandSwitch("--assignment")] string Assignment
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}