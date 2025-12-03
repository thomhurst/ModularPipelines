using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("managedservices", "assignment", "create")]
public record AzManagedservicesAssignmentCreateOptions(
[property: CliOption("--definition")] string Definition
) : AzOptions
{
    [CliOption("--assignment-id")]
    public string? AssignmentId { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}