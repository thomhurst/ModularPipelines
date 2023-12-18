using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "sub-assessment", "show")]
public record AzSecuritySubAssessmentShowOptions(
[property: CommandSwitch("--assessment-name")] string AssessmentName,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--assessed-resource-id")]
    public string? AssessedResourceId { get; set; }
}