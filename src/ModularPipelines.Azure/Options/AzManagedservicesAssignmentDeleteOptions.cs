using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("managedservices", "assignment", "delete")]
public record AzManagedservicesAssignmentDeleteOptions(
[property: CliOption("--assignment")] string Assignment
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}