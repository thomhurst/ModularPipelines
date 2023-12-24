using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auditmanager", "create-assessment-report")]
public record AwsAuditmanagerCreateAssessmentReportOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--assessment-id")] string AssessmentId
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--query-statement")]
    public string? QueryStatement { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}