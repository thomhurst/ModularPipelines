using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "delete-iam-policy-assignment")]
public record AwsQuicksightDeleteIamPolicyAssignmentOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--assignment-name")] string AssignmentName,
[property: CommandSwitch("--namespace")] string Namespace
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}