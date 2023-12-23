using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acm", "import-certificate")]
public record AwsAcmImportCertificateOptions(
[property: CommandSwitch("--private-key")] string PrivateKey
) : AwsOptions
{
    [CommandSwitch("--certificate-arn")]
    public string? CertificateArn { get; set; }

    [CommandSwitch("--certificate")]
    public string? Certificate { get; set; }

    [CommandSwitch("--certificate-chain")]
    public string? CertificateChain { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}