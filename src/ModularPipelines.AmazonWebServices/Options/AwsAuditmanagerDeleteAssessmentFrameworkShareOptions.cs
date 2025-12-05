using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auditmanager", "delete-assessment-framework-share")]
public record AwsAuditmanagerDeleteAssessmentFrameworkShareOptions(
[property: CliOption("--request-id")] string RequestId,
[property: CliOption("--request-type")] string RequestType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}