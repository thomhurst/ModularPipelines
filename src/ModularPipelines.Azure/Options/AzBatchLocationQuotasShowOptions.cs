using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "location", "quotas", "show")]
public record AzBatchLocationQuotasShowOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;