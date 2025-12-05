using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codepipeline", "deregister-webhook-with-third-party")]
public record AwsCodepipelineDeregisterWebhookWithThirdPartyOptions : AwsOptions
{
    [CliOption("--webhook-name")]
    public string? WebhookName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}