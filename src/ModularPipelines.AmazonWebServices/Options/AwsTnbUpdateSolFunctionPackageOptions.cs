using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tnb", "update-sol-function-package")]
public record AwsTnbUpdateSolFunctionPackageOptions(
[property: CommandSwitch("--operational-state")] string OperationalState,
[property: CommandSwitch("--vnf-pkg-id")] string VnfPkgId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}