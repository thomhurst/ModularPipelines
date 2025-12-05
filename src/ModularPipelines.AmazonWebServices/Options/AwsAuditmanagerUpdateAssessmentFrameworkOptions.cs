using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auditmanager", "update-assessment-framework")]
public record AwsAuditmanagerUpdateAssessmentFrameworkOptions(
[property: CliOption("--framework-id")] string FrameworkId,
[property: CliOption("--name")] string Name,
[property: CliOption("--control-sets")] string[] ControlSets
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--compliance-type")]
    public string? ComplianceType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}