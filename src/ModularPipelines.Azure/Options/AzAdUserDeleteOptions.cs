using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "user", "delete")]
public record AzAdUserDeleteOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions;