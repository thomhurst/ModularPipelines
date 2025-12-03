using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigatewayv2", "update-domain-name")]
public record AwsApigatewayv2UpdateDomainNameOptions(
[property: CliOption("--domain-name")] string DomainName
) : AwsOptions
{
    [CliOption("--domain-name-configurations")]
    public string[]? DomainNameConfigurations { get; set; }

    [CliOption("--mutual-tls-authentication")]
    public string? MutualTlsAuthentication { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}