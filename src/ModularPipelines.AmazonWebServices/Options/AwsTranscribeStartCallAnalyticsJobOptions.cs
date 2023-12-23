using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transcribe", "start-call-analytics-job")]
public record AwsTranscribeStartCallAnalyticsJobOptions(
[property: CommandSwitch("--call-analytics-job-name")] string CallAnalyticsJobName,
[property: CommandSwitch("--media")] string Media
) : AwsOptions
{
    [CommandSwitch("--output-location")]
    public string? OutputLocation { get; set; }

    [CommandSwitch("--output-encryption-kms-key-id")]
    public string? OutputEncryptionKmsKeyId { get; set; }

    [CommandSwitch("--data-access-role-arn")]
    public string? DataAccessRoleArn { get; set; }

    [CommandSwitch("--settings")]
    public string? Settings { get; set; }

    [CommandSwitch("--channel-definitions")]
    public string[]? ChannelDefinitions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}