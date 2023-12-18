using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managedservices", "assignment", "create")]
public record AzManagedservicesAssignmentCreateOptions(
[property: CommandSwitch("--definition")] string Definition
) : AzOptions
{
    [CommandSwitch("--assignment-id")]
    public string? AssignmentId { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}