using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "sp", "create")]
public record AzAdSpCreateOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions;