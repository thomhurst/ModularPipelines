using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managedservices", "definition", "delete")]
public record AzManagedservicesDefinitionDeleteOptions(
[property: CommandSwitch("--definition")] string Definition
) : AzOptions;