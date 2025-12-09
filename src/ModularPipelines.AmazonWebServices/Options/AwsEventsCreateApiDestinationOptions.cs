using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("events", "create-api-destination")]
public record AwsEventsCreateApiDestinationOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--connection-arn")] string ConnectionArn,
[property: CliOption("--invocation-endpoint")] string InvocationEndpoint,
[property: CliOption("--http-method")] string HttpMethod
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--invocation-rate-limit-per-second")]
    public int? InvocationRateLimitPerSecond { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}