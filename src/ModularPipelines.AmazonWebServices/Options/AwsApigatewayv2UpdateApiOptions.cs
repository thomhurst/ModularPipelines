using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigatewayv2", "update-api")]
public record AwsApigatewayv2UpdateApiOptions(
[property: CliOption("--api-id")] string ApiId
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

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--route-key")]
    public string? RouteKey { get; set; }

    [CliOption("--route-selection-expression")]
    public string? RouteSelectionExpression { get; set; }

    [CliOption("--target")]
    public string? Target { get; set; }

    [CliOption("--api-version")]
    public string? ApiVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}