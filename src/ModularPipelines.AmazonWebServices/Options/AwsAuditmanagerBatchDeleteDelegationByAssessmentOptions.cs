using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auditmanager", "batch-delete-delegation-by-assessment")]
public record AwsAuditmanagerBatchDeleteDelegationByAssessmentOptions(
[property: CliOption("--delegation-ids")] string[] DelegationIds,
[property: CliOption("--assessment-id")] string AssessmentId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}