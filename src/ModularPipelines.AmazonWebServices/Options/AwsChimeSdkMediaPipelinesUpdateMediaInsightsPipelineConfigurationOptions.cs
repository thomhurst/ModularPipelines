using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-media-pipelines", "update-media-insights-pipeline-configuration")]
public record AwsChimeSdkMediaPipelinesUpdateMediaInsightsPipelineConfigurationOptions(
[property: CliOption("--identifier")] string Identifier,
[property: CliOption("--resource-access-role-arn")] string ResourceAccessRoleArn,
[property: CliOption("--elements")] string[] Elements
) : AwsOptions
{
    [CliOption("--real-time-alert-configuration")]
    public string? RealTimeAlertConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}