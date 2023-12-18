using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "assessment", "delete")]
public record AzSecurityAssessmentDeleteOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--assessed-resource-id")]
    public string? AssessedResourceId { get; set; }
}