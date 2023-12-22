using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "account", "check-name")]
public record AzStorageAccountCheckNameOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;