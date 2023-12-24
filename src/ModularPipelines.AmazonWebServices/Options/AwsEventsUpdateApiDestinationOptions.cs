using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("events", "update-api-destination")]
public record AwsEventsUpdateApiDestinationOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--connection-arn")]
    public string? ConnectionArn { get; set; }

    [CommandSwitch("--invocation-endpoint")]
    public string? InvocationEndpoint { get; set; }

    [CommandSwitch("--http-method")]
    public string? HttpMethod { get; set; }

    [CommandSwitch("--invocation-rate-limit-per-second")]
    public int? InvocationRateLimitPerSecond { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}