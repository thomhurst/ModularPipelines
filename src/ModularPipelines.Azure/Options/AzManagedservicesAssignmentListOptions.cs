using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managedservices", "assignment", "list")]
public record AzManagedservicesAssignmentListOptions : AzOptions
{
    [BooleanCommandSwitch("--include-definition")]
    public bool? IncludeDefinition { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}