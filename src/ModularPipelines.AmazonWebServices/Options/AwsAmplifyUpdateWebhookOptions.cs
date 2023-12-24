using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("amplify", "update-webhook")]
public record AwsAmplifyUpdateWebhookOptions(
[property: CommandSwitch("--webhook-id")] string WebhookId
) : AwsOptions
{
    [CommandSwitch("--branch-name")]
    public string? BranchName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}