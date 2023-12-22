using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("extension", "show")]
public record AzExtensionShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;