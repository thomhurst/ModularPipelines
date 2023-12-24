using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "attach-certificate-to-distribution")]
public record AwsLightsailAttachCertificateToDistributionOptions(
[property: CommandSwitch("--distribution-name")] string DistributionName,
[property: CommandSwitch("--certificate-name")] string CertificateName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}