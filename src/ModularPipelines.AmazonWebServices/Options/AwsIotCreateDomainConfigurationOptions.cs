using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "create-domain-configuration")]
public record AwsIotCreateDomainConfigurationOptions(
[property: CommandSwitch("--domain-configuration-name")] string DomainConfigurationName
) : AwsOptions
{
    [CommandSwitch("--domain-name")]
    public string? DomainName { get; set; }

    [CommandSwitch("--server-certificate-arns")]
    public string[]? ServerCertificateArns { get; set; }

    [CommandSwitch("--validation-certificate-arn")]
    public string? ValidationCertificateArn { get; set; }

    [CommandSwitch("--authorizer-config")]
    public string? AuthorizerConfig { get; set; }

    [CommandSwitch("--service-type")]
    public string? ServiceType { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--tls-config")]
    public string? TlsConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}