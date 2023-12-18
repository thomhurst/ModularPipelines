using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "assessment-metadata", "show")]
public record AzSecurityAssessmentMetadataShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;