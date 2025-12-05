using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigatewayv2", "update-stage")]
public record AwsApigatewayv2UpdateStageOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--stage-name")] string StageName
) : AwsOptions
{
    [CliOption("--access-log-settings")]
    public string? AccessLogSettings { get; set; }

    [CliOption("--client-certificate-id")]
    public string? ClientCertificateId { get; set; }

    [CliOption("--default-route-settings")]
    public string? DefaultRouteSettings { get; set; }

    [CliOption("--deployment-id")]
    public string? DeploymentId { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--route-settings")]
    public IEnumerable<KeyValue>? RouteSettings { get; set; }

    [CliOption("--stage-variables")]
    public IEnumerable<KeyValue>? StageVariables { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}