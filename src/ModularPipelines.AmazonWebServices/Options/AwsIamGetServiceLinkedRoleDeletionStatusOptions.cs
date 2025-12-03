using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "get-service-linked-role-deletion-status")]
public record AwsIamGetServiceLinkedRoleDeletionStatusOptions(
[property: CliOption("--deletion-task-id")] string DeletionTaskId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}