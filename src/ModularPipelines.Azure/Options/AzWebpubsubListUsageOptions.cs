using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webpubsub", "list-usage")]
public record AzWebpubsubListUsageOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;