using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "update-iam-policy-assignment")]
public record AwsQuicksightUpdateIamPolicyAssignmentOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--assignment-name")] string AssignmentName,
[property: CommandSwitch("--namespace")] string Namespace
) : AwsOptions
{
    [CommandSwitch("--assignment-status")]
    public string? AssignmentStatus { get; set; }

    [CommandSwitch("--policy-arn")]
    public string? PolicyArn { get; set; }

    [CommandSwitch("--identities")]
    public IEnumerable<KeyValue>? Identities { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}