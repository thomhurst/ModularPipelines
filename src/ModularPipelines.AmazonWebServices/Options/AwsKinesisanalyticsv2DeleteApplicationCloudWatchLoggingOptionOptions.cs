using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisanalyticsv2", "delete-application-cloud-watch-logging-option")]
public record AwsKinesisanalyticsv2DeleteApplicationCloudWatchLoggingOptionOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--cloud-watch-logging-option-id")] string CloudWatchLoggingOptionId
) : AwsOptions
{
    [CommandSwitch("--current-application-version-id")]
    public long? CurrentApplicationVersionId { get; set; }

    [CommandSwitch("--conditional-token")]
    public string? ConditionalToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}