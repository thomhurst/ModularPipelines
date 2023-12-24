using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime", "put-events-configuration")]
public record AwsChimePutEventsConfigurationOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--bot-id")] string BotId
) : AwsOptions
{
    [CommandSwitch("--outbound-events-https-endpoint")]
    public string? OutboundEventsHttpsEndpoint { get; set; }

    [CommandSwitch("--lambda-function-arn")]
    public string? LambdaFunctionArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}