using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigatewayv2", "create-stage")]
public record AwsApigatewayv2CreateStageOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--stage-name")] string StageName
) : AwsOptions
{
    [CommandSwitch("--access-log-settings")]
    public string? AccessLogSettings { get; set; }

    [CommandSwitch("--client-certificate-id")]
    public string? ClientCertificateId { get; set; }

    [CommandSwitch("--default-route-settings")]
    public string? DefaultRouteSettings { get; set; }

    [CommandSwitch("--deployment-id")]
    public string? DeploymentId { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--route-settings")]
    public IEnumerable<KeyValue>? RouteSettings { get; set; }

    [CommandSwitch("--stage-variables")]
    public IEnumerable<KeyValue>? StageVariables { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}