using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auditmanager", "update-settings")]
public record AwsAuditmanagerUpdateSettingsOptions : AwsOptions
{
    [CommandSwitch("--sns-topic")]
    public string? SnsTopic { get; set; }

    [CommandSwitch("--default-assessment-reports-destination")]
    public string? DefaultAssessmentReportsDestination { get; set; }

    [CommandSwitch("--default-process-owners")]
    public string[]? DefaultProcessOwners { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--deregistration-policy")]
    public string? DeregistrationPolicy { get; set; }

    [CommandSwitch("--default-export-destination")]
    public string? DefaultExportDestination { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}