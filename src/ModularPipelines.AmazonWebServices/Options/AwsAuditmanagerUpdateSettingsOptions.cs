using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auditmanager", "update-settings")]
public record AwsAuditmanagerUpdateSettingsOptions : AwsOptions
{
    [CliOption("--sns-topic")]
    public string? SnsTopic { get; set; }

    [CliOption("--default-assessment-reports-destination")]
    public string? DefaultAssessmentReportsDestination { get; set; }

    [CliOption("--default-process-owners")]
    public string[]? DefaultProcessOwners { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--deregistration-policy")]
    public string? DeregistrationPolicy { get; set; }

    [CliOption("--default-export-destination")]
    public string? DefaultExportDestination { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}