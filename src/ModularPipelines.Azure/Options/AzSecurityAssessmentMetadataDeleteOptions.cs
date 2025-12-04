using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "assessment-metadata", "delete")]
public record AzSecurityAssessmentMetadataDeleteOptions(
[property: CliOption("--name")] string Name
) : AzOptions;