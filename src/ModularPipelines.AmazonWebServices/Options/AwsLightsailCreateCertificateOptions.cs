using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "create-certificate")]
public record AwsLightsailCreateCertificateOptions(
[property: CommandSwitch("--certificate-name")] string CertificateName,
[property: CommandSwitch("--domain-name")] string DomainName
) : AwsOptions
{
    [CommandSwitch("--subject-alternative-names")]
    public string[]? SubjectAlternativeNames { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}