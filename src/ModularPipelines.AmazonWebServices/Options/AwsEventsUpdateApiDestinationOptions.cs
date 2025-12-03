using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("events", "update-api-destination")]
public record AwsEventsUpdateApiDestinationOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--connection-arn")]
    public string? ConnectionArn { get; set; }

    [CliOption("--invocation-endpoint")]
    public string? InvocationEndpoint { get; set; }

    [CliOption("--http-method")]
    public string? HttpMethod { get; set; }

    [CliOption("--invocation-rate-limit-per-second")]
    public int? InvocationRateLimitPerSecond { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}