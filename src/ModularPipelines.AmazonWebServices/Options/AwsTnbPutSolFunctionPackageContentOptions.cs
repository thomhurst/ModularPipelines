using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tnb", "put-sol-function-package-content")]
public record AwsTnbPutSolFunctionPackageContentOptions(
[property: CommandSwitch("--file")] string File,
[property: CommandSwitch("--vnf-pkg-id")] string VnfPkgId
) : AwsOptions
{
    [CommandSwitch("--content-type")]
    public string? ContentType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}