using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tnb", "get-sol-network-package-descriptor")]
public record AwsTnbGetSolNetworkPackageDescriptorOptions(
[property: CommandSwitch("--nsd-info-id")] string NsdInfoId
) : AwsOptions;