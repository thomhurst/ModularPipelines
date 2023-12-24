using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-media-pipelines", "create-media-insights-pipeline-configuration")]
public record AwsChimeSdkMediaPipelinesCreateMediaInsightsPipelineConfigurationOptions(
[property: CommandSwitch("--media-insights-pipeline-configuration-name")] string MediaInsightsPipelineConfigurationName,
[property: CommandSwitch("--resource-access-role-arn")] string ResourceAccessRoleArn,
[property: CommandSwitch("--elements")] string[] Elements
) : AwsOptions
{
    [CommandSwitch("--real-time-alert-configuration")]
    public string? RealTimeAlertConfiguration { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}