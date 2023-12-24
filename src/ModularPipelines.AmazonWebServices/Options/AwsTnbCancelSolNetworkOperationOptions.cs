using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tnb", "cancel-sol-network-operation")]
public record AwsTnbCancelSolNetworkOperationOptions(
[property: CommandSwitch("--ns-lcm-op-occ-id")] string NsLcmOpOccId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}