using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tnb", "get-sol-network-operation")]
public record AwsTnbGetSolNetworkOperationOptions(
[property: CommandSwitch("--ns-lcm-op-occ-id")] string NsLcmOpOccId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}