using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloud", "unregister")]
public record AzCloudUnregisterOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;