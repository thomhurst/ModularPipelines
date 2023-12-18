using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "assessment-metadata", "delete")]
public record AzSecurityAssessmentMetadataDeleteOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;