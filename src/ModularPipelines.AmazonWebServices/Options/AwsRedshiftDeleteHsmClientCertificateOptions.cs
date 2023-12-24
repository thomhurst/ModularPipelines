using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "delete-hsm-client-certificate")]
public record AwsRedshiftDeleteHsmClientCertificateOptions(
[property: CommandSwitch("--hsm-client-certificate-identifier")] string HsmClientCertificateIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}