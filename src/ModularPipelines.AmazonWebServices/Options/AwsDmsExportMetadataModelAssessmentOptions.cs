using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "export-metadata-model-assessment")]
public record AwsDmsExportMetadataModelAssessmentOptions(
[property: CommandSwitch("--migration-project-identifier")] string MigrationProjectIdentifier,
[property: CommandSwitch("--selection-rules")] string SelectionRules
) : AwsOptions
{
    [CommandSwitch("--file-name")]
    public string? FileName { get; set; }

    [CommandSwitch("--assessment-report-types")]
    public string[]? AssessmentReportTypes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}