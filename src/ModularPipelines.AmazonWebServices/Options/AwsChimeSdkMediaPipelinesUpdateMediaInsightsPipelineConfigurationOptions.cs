using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-media-pipelines", "update-media-insights-pipeline-configuration")]
public record AwsChimeSdkMediaPipelinesUpdateMediaInsightsPipelineConfigurationOptions(
[property: CommandSwitch("--identifier")] string Identifier,
[property: CommandSwitch("--resource-access-role-arn")] string ResourceAccessRoleArn,
[property: CommandSwitch("--elements")] string[] Elements
) : AwsOptions
{
    [CommandSwitch("--real-time-alert-configuration")]
    public string? RealTimeAlertConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}