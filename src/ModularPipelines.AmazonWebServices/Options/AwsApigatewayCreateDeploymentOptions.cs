using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "create-deployment")]
public record AwsApigatewayCreateDeploymentOptions(
[property: CliOption("--rest-api-id")] string RestApiId
) : AwsOptions
{
    [CliOption("--stage-name")]
    public string? StageName { get; set; }

    [CliOption("--stage-description")]
    public string? StageDescription { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--cache-cluster-size")]
    public string? CacheClusterSize { get; set; }

    [CliOption("--variables")]
    public IEnumerable<KeyValue>? Variables { get; set; }

    [CliOption("--canary-settings")]
    public string? CanarySettings { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}