using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "reject-certificate-transfer")]
public record AwsIotRejectCertificateTransferOptions(
[property: CommandSwitch("--certificate-id")] string CertificateId
) : AwsOptions
{
    [CommandSwitch("--reject-reason")]
    public string? RejectReason { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}