using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "assessment-metadata", "list")]
public record AzSecurityAssessmentMetadataListOptions : AzOptions;