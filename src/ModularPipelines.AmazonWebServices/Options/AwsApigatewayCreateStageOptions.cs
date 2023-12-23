using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigateway", "create-stage")]
public record AwsApigatewayCreateStageOptions(
[property: CommandSwitch("--rest-api-id")] string RestApiId,
[property: CommandSwitch("--stage-name")] string StageName,
[property: CommandSwitch("--deployment-id")] string DeploymentId
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--cache-cluster-size")]
    public string? CacheClusterSize { get; set; }

    [CommandSwitch("--variables")]
    public IEnumerable<KeyValue>? Variables { get; set; }

    [CommandSwitch("--documentation-version")]
    public string? DocumentationVersion { get; set; }

    [CommandSwitch("--canary-settings")]
    public string? CanarySettings { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}