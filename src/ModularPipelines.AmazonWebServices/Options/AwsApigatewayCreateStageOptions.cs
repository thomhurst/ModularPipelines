using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "create-stage")]
public record AwsApigatewayCreateStageOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--stage-name")] string StageName,
[property: CliOption("--deployment-id")] string DeploymentId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--cache-cluster-size")]
    public string? CacheClusterSize { get; set; }

    [CliOption("--variables")]
    public IEnumerable<KeyValue>? Variables { get; set; }

    [CliOption("--documentation-version")]
    public string? DocumentationVersion { get; set; }

    [CliOption("--canary-settings")]
    public string? CanarySettings { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}