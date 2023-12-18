using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "assessment-metadata", "list")]
public record AzSecurityAssessmentMetadataListOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}