using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "user", "show")]
public record AzAdUserShowOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions;