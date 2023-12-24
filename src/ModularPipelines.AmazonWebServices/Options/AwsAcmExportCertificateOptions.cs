using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acm", "export-certificate")]
public record AwsAcmExportCertificateOptions(
[property: CommandSwitch("--certificate-arn")] string CertificateArn,
[property: CommandSwitch("--passphrase")] string Passphrase
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}