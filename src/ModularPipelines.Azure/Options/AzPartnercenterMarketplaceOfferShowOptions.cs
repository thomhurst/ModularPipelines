using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("partnercenter", "marketplace", "offer", "show")]
public record AzPartnercenterMarketplaceOfferShowOptions(
[property: CliOption("--id")] string Id
) : AzOptions;