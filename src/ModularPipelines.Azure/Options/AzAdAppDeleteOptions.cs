using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "app", "delete")]
public record AzAdAppDeleteOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions;