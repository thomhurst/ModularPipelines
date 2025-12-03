using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spring", "list-marketplace-plan")]
public record AzSpringListMarketplacePlanOptions : AzOptions;