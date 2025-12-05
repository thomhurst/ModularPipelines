using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "transfer-certificate")]
public record AwsIotTransferCertificateOptions(
[property: CliOption("--certificate-id")] string CertificateId,
[property: CliOption("--target-aws-account")] string TargetAwsAccount
) : AwsOptions
{
    [CliOption("--transfer-message")]
    public string? TransferMessage { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}