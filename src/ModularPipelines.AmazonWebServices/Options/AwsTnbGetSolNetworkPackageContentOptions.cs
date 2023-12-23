using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tnb", "get-sol-network-package-content")]
public record AwsTnbGetSolNetworkPackageContentOptions(
[property: CommandSwitch("--accept")] string Accept,
[property: CommandSwitch("--nsd-info-id")] string NsdInfoId
) : AwsOptions;