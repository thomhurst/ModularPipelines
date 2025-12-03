using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "assessment-metadata", "delete")]
public record AzSecurityAssessmentMetadataDeleteOptions(
[property: CliOption("--name")] string Name
) : AzOptions;