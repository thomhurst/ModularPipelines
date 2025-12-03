using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "assessment-metadata", "show")]
public record AzSecurityAssessmentMetadataShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions;