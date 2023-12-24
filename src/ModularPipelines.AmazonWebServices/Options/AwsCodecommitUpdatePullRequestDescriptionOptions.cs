using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "update-pull-request-description")]
public record AwsCodecommitUpdatePullRequestDescriptionOptions(
[property: CommandSwitch("--pull-request-id")] string PullRequestId,
[property: CommandSwitch("--description")] string Description
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}