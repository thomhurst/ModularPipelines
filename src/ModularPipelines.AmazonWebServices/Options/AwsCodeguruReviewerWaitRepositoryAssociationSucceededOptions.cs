using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeguru-reviewer", "wait", "repository-association-succeeded")]
public record AwsCodeguruReviewerWaitRepositoryAssociationSucceededOptions(
[property: CommandSwitch("--association-arn")] string AssociationArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}