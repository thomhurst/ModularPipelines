using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "create-domain-configuration")]
public record AwsIotCreateDomainConfigurationOptions(
[property: CliOption("--domain-configuration-name")] string DomainConfigurationName
) : AwsOptions
{
    [CliOption("--domain-name")]
    public string? DomainName { get; set; }

    [CliOption("--server-certificate-arns")]
    public string[]? ServerCertificateArns { get; set; }

    [CliOption("--validation-certificate-arn")]
    public string? ValidationCertificateArn { get; set; }

    [CliOption("--authorizer-config")]
    public string? AuthorizerConfig { get; set; }

    [CliOption("--service-type")]
    public string? ServiceType { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--tls-config")]
    public string? TlsConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}