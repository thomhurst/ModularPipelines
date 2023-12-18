using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "sp", "delete")]
public record AzAdSpDeleteOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions;