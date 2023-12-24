using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "create-iam-policy-assignment")]
public record AwsQuicksightCreateIamPolicyAssignmentOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--assignment-name")] string AssignmentName,
[property: CommandSwitch("--assignment-status")] string AssignmentStatus,
[property: CommandSwitch("--namespace")] string Namespace
) : AwsOptions
{
    [CommandSwitch("--policy-arn")]
    public string? PolicyArn { get; set; }

    [CommandSwitch("--identities")]
    public IEnumerable<KeyValue>? Identities { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}