using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "enable-address-transfer")]
public record AwsEc2EnableAddressTransferOptions(
[property: CommandSwitch("--allocation-id")] string AllocationId,
[property: CommandSwitch("--transfer-account-id")] string TransferAccountId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}