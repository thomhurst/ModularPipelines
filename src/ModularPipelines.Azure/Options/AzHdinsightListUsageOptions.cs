using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hdinsight", "list-usage")]
public record AzHdinsightListUsageOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;