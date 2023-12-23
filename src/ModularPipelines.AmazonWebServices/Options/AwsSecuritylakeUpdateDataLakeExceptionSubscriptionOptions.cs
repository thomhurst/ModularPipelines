using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securitylake", "update-data-lake-exception-subscription")]
public record AwsSecuritylakeUpdateDataLakeExceptionSubscriptionOptions(
[property: CommandSwitch("--notification-endpoint")] string NotificationEndpoint,
[property: CommandSwitch("--subscription-protocol")] string SubscriptionProtocol
) : AwsOptions
{
    [CommandSwitch("--exception-time-to-live")]
    public long? ExceptionTimeToLive { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}