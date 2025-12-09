using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tnb", "get-sol-network-package-content")]
public record AwsTnbGetSolNetworkPackageContentOptions(
[property: CliOption("--accept")] string Accept,
[property: CliOption("--nsd-info-id")] string NsdInfoId
) : AwsOptions;