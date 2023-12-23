using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigateway", "create-deployment")]
public record AwsApigatewayCreateDeploymentOptions(
[property: CommandSwitch("--rest-api-id")] string RestApiId
) : AwsOptions
{
    [CommandSwitch("--stage-name")]
    public string? StageName { get; set; }

    [CommandSwitch("--stage-description")]
    public string? StageDescription { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--cache-cluster-size")]
    public string? CacheClusterSize { get; set; }

    [CommandSwitch("--variables")]
    public IEnumerable<KeyValue>? Variables { get; set; }

    [CommandSwitch("--canary-settings")]
    public string? CanarySettings { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}