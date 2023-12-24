using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "delete-certificate-provider")]
public record AwsIotDeleteCertificateProviderOptions(
[property: CommandSwitch("--certificate-provider-name")] string CertificateProviderName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}