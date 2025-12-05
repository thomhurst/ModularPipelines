using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigatewayv2", "delete-domain-name")]
public record AwsApigatewayv2DeleteDomainNameOptions(
[property: CliOption("--domain-name")] string DomainName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}