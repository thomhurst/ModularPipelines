using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("events", "create-api-destination")]
public record AwsEventsCreateApiDestinationOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--connection-arn")] string ConnectionArn,
[property: CommandSwitch("--invocation-endpoint")] string InvocationEndpoint,
[property: CommandSwitch("--http-method")] string HttpMethod
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--invocation-rate-limit-per-second")]
    public int? InvocationRateLimitPerSecond { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}