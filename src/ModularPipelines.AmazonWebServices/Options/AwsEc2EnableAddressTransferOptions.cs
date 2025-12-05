using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "enable-address-transfer")]
public record AwsEc2EnableAddressTransferOptions(
[property: CliOption("--allocation-id")] string AllocationId,
[property: CliOption("--transfer-account-id")] string TransferAccountId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}