using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("extension", "list-versions")]
public record AzExtensionListVersionsOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;