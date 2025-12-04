using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("partnercenter", "marketplace", "offer", "publish")]
public record AzPartnercenterMarketplaceOfferPublishOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--target")] string Target
) : AzOptions;