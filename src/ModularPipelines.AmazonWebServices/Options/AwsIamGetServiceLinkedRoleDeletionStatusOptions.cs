using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "get-service-linked-role-deletion-status")]
public record AwsIamGetServiceLinkedRoleDeletionStatusOptions(
[property: CommandSwitch("--deletion-task-id")] string DeletionTaskId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}