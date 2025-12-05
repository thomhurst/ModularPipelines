using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "start-metadata-model-assessment")]
public record AwsDmsStartMetadataModelAssessmentOptions(
[property: CliOption("--migration-project-identifier")] string MigrationProjectIdentifier,
[property: CliOption("--selection-rules")] string SelectionRules
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}