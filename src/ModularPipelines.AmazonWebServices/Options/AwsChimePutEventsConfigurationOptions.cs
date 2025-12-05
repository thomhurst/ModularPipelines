using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime", "put-events-configuration")]
public record AwsChimePutEventsConfigurationOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--bot-id")] string BotId
) : AwsOptions
{
    [CliOption("--outbound-events-https-endpoint")]
    public string? OutboundEventsHttpsEndpoint { get; set; }

    [CliOption("--lambda-function-arn")]
    public string? LambdaFunctionArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}