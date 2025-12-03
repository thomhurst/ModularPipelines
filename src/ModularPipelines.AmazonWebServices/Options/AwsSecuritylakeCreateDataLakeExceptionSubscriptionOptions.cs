using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securitylake", "create-data-lake-exception-subscription")]
public record AwsSecuritylakeCreateDataLakeExceptionSubscriptionOptions(
[property: CliOption("--notification-endpoint")] string NotificationEndpoint,
[property: CliOption("--subscription-protocol")] string SubscriptionProtocol
) : AwsOptions
{
    [CliOption("--exception-time-to-live")]
    public long? ExceptionTimeToLive { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}