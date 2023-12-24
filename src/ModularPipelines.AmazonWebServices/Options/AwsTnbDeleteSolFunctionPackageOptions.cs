using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tnb", "delete-sol-function-package")]
public record AwsTnbDeleteSolFunctionPackageOptions(
[property: CommandSwitch("--vnf-pkg-id")] string VnfPkgId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}