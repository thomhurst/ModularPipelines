using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "contact", "delete")]
public record AzSecurityContactDeleteOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;