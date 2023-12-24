using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "transfer-certificate")]
public record AwsIotTransferCertificateOptions(
[property: CommandSwitch("--certificate-id")] string CertificateId,
[property: CommandSwitch("--target-aws-account")] string TargetAwsAccount
) : AwsOptions
{
    [CommandSwitch("--transfer-message")]
    public string? TransferMessage { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}