using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managedapp", "definition", "list")]
public record AzManagedappDefinitionListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;