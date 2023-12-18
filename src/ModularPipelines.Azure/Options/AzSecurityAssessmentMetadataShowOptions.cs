using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "assessment-metadata", "show")]
public record AzSecurityAssessmentMetadataShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}