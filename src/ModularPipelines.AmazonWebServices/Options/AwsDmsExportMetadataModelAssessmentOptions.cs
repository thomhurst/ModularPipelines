using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "export-metadata-model-assessment")]
public record AwsDmsExportMetadataModelAssessmentOptions(
[property: CliOption("--migration-project-identifier")] string MigrationProjectIdentifier,
[property: CliOption("--selection-rules")] string SelectionRules
) : AwsOptions
{
    [CliOption("--file-name")]
    public string? FileName { get; set; }

    [CliOption("--assessment-report-types")]
    public string[]? AssessmentReportTypes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}