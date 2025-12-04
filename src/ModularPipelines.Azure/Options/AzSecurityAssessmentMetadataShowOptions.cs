using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "assessment-metadata", "show")]
public record AzSecurityAssessmentMetadataShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions;