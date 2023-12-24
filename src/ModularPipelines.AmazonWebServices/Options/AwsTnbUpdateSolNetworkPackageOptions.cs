using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tnb", "update-sol-network-package")]
public record AwsTnbUpdateSolNetworkPackageOptions(
[property: CommandSwitch("--nsd-info-id")] string NsdInfoId,
[property: CommandSwitch("--nsd-operational-state")] string NsdOperationalState
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}