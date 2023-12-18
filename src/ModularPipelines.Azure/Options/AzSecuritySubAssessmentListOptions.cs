using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "sub-assessment", "list")]
public record AzSecuritySubAssessmentListOptions : AzOptions
{
    [CommandSwitch("--assessed-resource-id")]
    public string? AssessedResourceId { get; set; }

    [CommandSwitch("--assessment-name")]
    public string? AssessmentName { get; set; }
}