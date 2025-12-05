using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auditmanager", "update-assessment-framework-share")]
public record AwsAuditmanagerUpdateAssessmentFrameworkShareOptions(
[property: CliOption("--request-id")] string RequestId,
[property: CliOption("--request-type")] string RequestType,
[property: CliOption("--action")] string Action
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}