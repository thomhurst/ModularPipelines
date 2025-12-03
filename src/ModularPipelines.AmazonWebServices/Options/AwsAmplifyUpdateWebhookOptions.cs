using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplify", "update-webhook")]
public record AwsAmplifyUpdateWebhookOptions(
[property: CliOption("--webhook-id")] string WebhookId
) : AwsOptions
{
    [CliOption("--branch-name")]
    public string? BranchName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}