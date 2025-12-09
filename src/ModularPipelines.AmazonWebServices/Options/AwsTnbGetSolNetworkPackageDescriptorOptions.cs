using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tnb", "get-sol-network-package-descriptor")]
public record AwsTnbGetSolNetworkPackageDescriptorOptions(
[property: CliOption("--nsd-info-id")] string NsdInfoId
) : AwsOptions;