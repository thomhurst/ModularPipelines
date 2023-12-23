using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codepipeline", "register-webhook-with-third-party")]
public record AwsCodepipelineRegisterWebhookWithThirdPartyOptions : AwsOptions
{
    [CommandSwitch("--webhook-name")]
    public string? WebhookName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}