using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("managedservices", "assignment", "show")]
public record AzManagedservicesAssignmentShowOptions(
[property: CliOption("--assignment")] string Assignment
) : AzOptions
{
    [CliFlag("--include-definition")]
    public bool? IncludeDefinition { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}