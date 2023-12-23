using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auditmanager", "batch-delete-delegation-by-assessment")]
public record AwsAuditmanagerBatchDeleteDelegationByAssessmentOptions(
[property: CommandSwitch("--delegation-ids")] string[] DelegationIds,
[property: CommandSwitch("--assessment-id")] string AssessmentId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}