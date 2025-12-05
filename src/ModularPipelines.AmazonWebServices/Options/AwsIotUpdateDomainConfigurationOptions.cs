using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "update-domain-configuration")]
public record AwsIotUpdateDomainConfigurationOptions(
[property: CliOption("--domain-configuration-name")] string DomainConfigurationName
) : AwsOptions
{
    [CliOption("--authorizer-config")]
    public string? AuthorizerConfig { get; set; }

    [CliOption("--domain-configuration-status")]
    public string? DomainConfigurationStatus { get; set; }

    [CliOption("--tls-config")]
    public string? TlsConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}