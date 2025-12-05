using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-media-pipelines", "create-media-insights-pipeline-configuration")]
public record AwsChimeSdkMediaPipelinesCreateMediaInsightsPipelineConfigurationOptions(
[property: CliOption("--media-insights-pipeline-configuration-name")] string MediaInsightsPipelineConfigurationName,
[property: CliOption("--resource-access-role-arn")] string ResourceAccessRoleArn,
[property: CliOption("--elements")] string[] Elements
) : AwsOptions
{
    [CliOption("--real-time-alert-configuration")]
    public string? RealTimeAlertConfiguration { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}