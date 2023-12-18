using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("extension", "remove")]
public record AzExtensionRemoveOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;