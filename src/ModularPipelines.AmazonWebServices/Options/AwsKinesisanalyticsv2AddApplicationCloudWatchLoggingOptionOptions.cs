using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisanalyticsv2", "add-application-cloud-watch-logging-option")]
public record AwsKinesisanalyticsv2AddApplicationCloudWatchLoggingOptionOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--cloud-watch-logging-option")] string CloudWatchLoggingOption
) : AwsOptions
{
    [CommandSwitch("--current-application-version-id")]
    public long? CurrentApplicationVersionId { get; set; }

    [CommandSwitch("--conditional-token")]
    public string? ConditionalToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}