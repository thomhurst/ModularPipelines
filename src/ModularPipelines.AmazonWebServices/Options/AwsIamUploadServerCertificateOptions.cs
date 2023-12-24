using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "upload-server-certificate")]
public record AwsIamUploadServerCertificateOptions(
[property: CommandSwitch("--server-certificate-name")] string ServerCertificateName,
[property: CommandSwitch("--certificate-body")] string CertificateBody,
[property: CommandSwitch("--private-key")] string PrivateKey
) : AwsOptions
{
    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--certificate-chain")]
    public string? CertificateChain { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}