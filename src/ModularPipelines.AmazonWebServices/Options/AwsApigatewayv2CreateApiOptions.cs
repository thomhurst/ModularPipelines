using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigatewayv2", "create-api")]
public record AwsApigatewayv2CreateApiOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--protocol-type")] string ProtocolType
) : AwsOptions
{
    [CliOption("--api-key-selection-expression")]
    public string? ApiKeySelectionExpression { get; set; }

    [CliOption("--cors-configuration")]
    public string? CorsConfiguration { get; set; }

    [CliOption("--credentials-arn")]
    public string? CredentialsArn { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--route-key")]
    public string? RouteKey { get; set; }

    [CliOption("--route-selection-expression")]
    public string? RouteSelectionExpression { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--target")]
    public string? Target { get; set; }

    [CliOption("--api-version")]
    public string? ApiVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}