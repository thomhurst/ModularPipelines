using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("partnercenter", "marketplace", "offer", "setup", "show")]
public record AzPartnercenterMarketplaceOfferSetupShowOptions(
[property: CliOption("--id")] string Id
) : AzOptions;