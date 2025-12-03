using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("managedservices", "assignment", "list")]
public record AzManagedservicesAssignmentListOptions : AzOptions
{
    [CliFlag("--include-definition")]
    public bool? IncludeDefinition { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}