using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auditmanager", "batch-create-delegation-by-assessment")]
public record AwsAuditmanagerBatchCreateDelegationByAssessmentOptions(
[property: CommandSwitch("--create-delegation-requests")] string[] CreateDelegationRequests,
[property: CommandSwitch("--assessment-id")] string AssessmentId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}