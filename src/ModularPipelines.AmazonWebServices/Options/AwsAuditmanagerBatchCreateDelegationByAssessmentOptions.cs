using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auditmanager", "batch-create-delegation-by-assessment")]
public record AwsAuditmanagerBatchCreateDelegationByAssessmentOptions(
[property: CliOption("--create-delegation-requests")] string[] CreateDelegationRequests,
[property: CliOption("--assessment-id")] string AssessmentId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}