using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring", "list-marketplace-plan")]
public record AzSpringListMarketplacePlanOptions : AzOptions;